# CMakeList.txt : CMake project for TestCMake, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)

project ("cMsgPack")

add_library(cMsgPack SHARED "cMsgPack.c")
install(TARGETS cMsgPack LIBRARY DESTINATION /lib)
