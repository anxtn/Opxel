cmake_minimum_required(VERSION 3.16)
project(Opxel)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)
set(CMAKE_EXPORT_COMPILE_COMMANDS ON)

find_package(spdlog REQUIRED)

find_package(glm REQUIRED)

find_package(Vulkan REQUIRED)

find_package(PkgConfig REQUIRED)
pkg_check_modules(GLFW REQUIRED glfw3)

add_executable(OpxelApp 
    src/main.cpp
    src/graphics/window.cpp
    src/graphics/renderer.cpp
    src/graphics/instance.cpp
    src/graphics/vulkanUtils.cpp
    src/graphics/debugMessenger.cpp
    src/graphics/physicalDevicePicker.cpp
    src/graphics/physicalDeviceInfo.cpp
    src/logger.cpp
)

target_compile_features(OpxelApp PUBLIC cxx_std_20)

target_include_directories(OpxelApp PRIVATE
    ${Vulkan_INCLUDE_DIRS}
    ${GLFW_INCLUDE_DIRS}
)

target_link_libraries(OpxelApp PRIVATE
    spdlog::spdlog
    Vulkan::Vulkan
    glm::glm
    ${GLFW_LIBRARIES}
)
