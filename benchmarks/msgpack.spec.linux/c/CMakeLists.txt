# CMakeList.txt : CMake project for TestCMake, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)
SET(CMAKE_CXX_FLAGS "-O2")
SET(CMAKE_C_FLAGS "-O2")

project ("cMsgPack")

# Taken here: https://github.com/tarantool/tarantool/blob/2.0/cmake/FindMsgPuck.cmake
# - Find libmsgpuck header-only library
find_path(MSGPUCK_INCLUDE_DIR msgpuck.h PATH_SUFFIXES msgpuck)
find_library(MSGPUCK_LIBRARY NAMES libmsgpuck.a)

include(FindPackageHandleStandardArgs)
find_package_handle_standard_args(MsgPuck REQUIRED_VARS MSGPUCK_INCLUDE_DIR MSGPUCK_LIBRARY)
set(MSGPUCK_INCLUDE_DIRS ${MSGPUCK_INCLUDE_DIR})
set(MSGPUCK_LIBRARIES ${MSGPUCK_LIBRARY})
mark_as_advanced(MSGPUCK_INCLUDE_DIR MSGPUCK_INCLUDE_DIRS
MSGPUCK_LIBRARY MSGPUCK_LIBRARIES)

include_directories(MSGPUCK_INCLUDE_DIR)

add_library(cMsgPack SHARED "cMsgPack.c")
add_library(cppMsgPack SHARED "cppMsgPack.cpp")

target_link_libraries(cMsgPack ${MSGPUCK_LIBRARIES})
install(TARGETS cMsgPack LIBRARY DESTINATION /lib)
install(TARGETS cppMsgPack LIBRARY DESTINATION /lib)
