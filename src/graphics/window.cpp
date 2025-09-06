#include "window.hpp"
#include "../logger.hpp"
#include <GLFW/glfw3.h>
#include <format>

namespace {
void glfwErrorCallback(int error, const char *descriptor) {
    std::string desc = descriptor ? descriptor : "[no description]";
    std::string errorString =
        std::format("GLFW Error({}): {}", error, descriptor);
    logging::error(errorString);
}
bool setupGLFW() {
    glfwSetErrorCallback(glfwErrorCallback);
    return glfwInit();
}
} // namespace

namespace graphics {

Window::Window(const WindowSettings &settings) {
    if (!setupGLFW())
        return;
    int width = static_cast<int>(settings.windowSize.x);
    int height = static_cast<int>(settings.windowSize.y);
    const char *title = settings.title.c_str();
    glfwWindowHint(GLFW_CLIENT_API, GLFW_NO_API);
    window_ = glfwCreateWindow(width, height, title, nullptr, nullptr);
    if (!window_) {
        auto msg =
            std::format("Window creation failed (title = {})", settings.title);
        throw std::runtime_error(msg);
    } else {
        auto msg = std::format("Created window (title: \"{}\", ptr: {:#X})",
                               getTitle(), reinterpret_cast<intptr_t>(window_));
        logging::debug(msg);
    }
}

Window::~Window() {
    glfwDestroyWindow(window_);
}

bool Window::shouldClose() const {
    return glfwWindowShouldClose(window_);
}

void Window::pollEvents() const {
    glfwPollEvents();
}

bool Window::isFocused() const {
    return glfwGetWindowAttrib(window_, GLFW_FOCUSED);
}

glm::uvec2 Window::getWindowSize() const {
    int width, height;
    glfwGetWindowSize(window_, &width, &height);
    return glm::uvec2(width, height);
}

glm::uvec2 Window::getFramebufferSize() const {
    int width, height;
    glfwGetFramebufferSize(window_, &width, &height);
    return glm::uvec2(width, height);
}

glm::uvec2 Window::getPosition() const {
    int x, y;
    glfwGetWindowPos(window_, &y, &x);
    return glm::uvec2(y, x);
}

std::string Window::getTitle() const {
    const char *title = glfwGetWindowTitle(window_);
    return title ? std::string(title) : "";
}

std::string Window::getInfoStr() const {
    glm::uvec2 windowSize = getWindowSize();
    glm::uvec2 bufferSize = getFramebufferSize();
    return std::format("---window info---\n"
                       "Is Focused:  {}\n"
                       "Title:       {}\n"
                       "Window Size: {}x{}\n"
                       "Buffer Size: {}x{}\n",
                       isFocused(), getTitle(), windowSize.x, windowSize.y,
                       bufferSize.x, bufferSize.y);
}

std::vector<const char *> Window::getGLFWVulkanExtensions() const {
    uint32_t count = 0;
    const char **extensions = glfwGetRequiredInstanceExtensions(&count);
    return std::vector<const char *>(extensions, extensions + count);
}
} // namespace graphics
