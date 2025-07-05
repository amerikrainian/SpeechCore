# CMakeLists.txt for SpeechCore integration into VCMI

project(SpeechCore)

# Define source files based on platform
set(SpeechCore_COMMON_SRCS
    src/SpeechCore.cpp
)

set(SpeechCore_HEADERS
    include/SpeechCore.h
    src/SCDrivers/SCDriver.h
    src/SCDrivers/drivers.h
)

if(WIN32)
    set(SpeechCore_PLATFORM_SRCS
        src/SCDrivers/jaws.cpp
        src/SCDrivers/nvda.cpp
        src/SCDrivers/sa.cpp
        src/SCDrivers/sapi5driver.cpp
        src/SCDrivers/pc_talker.cpp
        src/SCDrivers/zdsr.cpp
        src/wrappers/SapiSpeech.cpp
        src/wrappers/saapi.cpp
        src/wrappers/zdsrapi.cpp
        src/ThirdParty/fsapi.c
        src/ThirdParty/wasapi.cpp
    )
    set(SpeechCore_PLATFORM_LIBS sapi ole32 user32 oleaut32)
elseif(APPLE)
    set(SpeechCore_PLATFORM_SRCS
        src/SCDrivers/AVTts.cpp
        src/wrappers/AVSpeech.mm
    )
    set(SpeechCore_PLATFORM_LIBS
        "-framework AVFoundation"
        "-framework Foundation"
        "-lobjc"
    )
    # Enable Objective-C++ for .mm files
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -x objective-c++")
elseif(UNIX)
    set(SpeechCore_PLATFORM_SRCS
        src/SCDrivers/SpeechDispatcher.cpp
    )
    # Find speech-dispatcher
    find_package(PkgConfig)
    if(PkgConfig_FOUND)
        pkg_check_modules(SPEECHD speech-dispatcher)
        if(SPEECHD_FOUND)
            set(SpeechCore_PLATFORM_LIBS ${SPEECHD_LIBRARIES})
            set(SpeechCore_PLATFORM_INCLUDE_DIRS ${SPEECHD_INCLUDE_DIRS})
        else()
            message(WARNING "speech-dispatcher not found. Speech functionality will be limited on Linux.")
        endif()
    endif()
endif()

# Create the library
add_library(SpeechCore STATIC
    ${SpeechCore_COMMON_SRCS}
    ${SpeechCore_PLATFORM_SRCS}
    ${SpeechCore_HEADERS}
)

# Define __SPEECH_C_STATIC for static library build
target_compile_definitions(SpeechCore PUBLIC __SPEECH_C_STATIC)

# Set include directories
target_include_directories(SpeechCore
    PUBLIC
        $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
        $<INSTALL_INTERFACE:include>
    PRIVATE
        ${CMAKE_CURRENT_SOURCE_DIR}/src
        ${CMAKE_CURRENT_SOURCE_DIR}/src/SCDrivers
        ${CMAKE_CURRENT_SOURCE_DIR}/src/ThirdParty
        ${CMAKE_CURRENT_SOURCE_DIR}/src/wrappers
        ${SpeechCore_PLATFORM_INCLUDE_DIRS}
)

# Platform-specific compile definitions
if(WIN32)
    target_compile_definitions(SpeechCore PRIVATE _WINDOWS)
elseif(APPLE)
    target_compile_definitions(SpeechCore PRIVATE __APPLE__)
elseif(UNIX)
    target_compile_definitions(SpeechCore PRIVATE __linux__)
endif()

# Link platform-specific libraries
if(SpeechCore_PLATFORM_LIBS)
    target_link_libraries(SpeechCore PUBLIC ${SpeechCore_PLATFORM_LIBS})
endif()

# Set properties
set_target_properties(SpeechCore PROPERTIES
    CXX_STANDARD 20
    CXX_STANDARD_REQUIRED ON
    POSITION_INDEPENDENT_CODE ON
)

# Handle Objective-C++ files on macOS
if(APPLE)
    set_source_files_properties(src/wrappers/AVSpeech.mm PROPERTIES 
        COMPILE_FLAGS "-x objective-c++"
        LANGUAGE CXX
    )
endif()

# Export target for VCMI to use
add_library(SpeechCore::SpeechCore ALIAS SpeechCore)

# Set folder for Visual Studio
set_target_properties(SpeechCore PROPERTIES FOLDER "3rdparty")