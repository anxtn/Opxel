#pragma once
#include <glm/vec2.hpp>
#include <string>
#include <vector>

struct GLFWwindow;

namespace graphics {

struct WindowSettings {
    glm::uvec2 windowSize{800, 600};
    std::string title = "Untitled Window";
};

class Window {
  public:
    Window(const WindowSettings &settings);
    ~Window();
    bool shouldClose() const;
    void pollEvents() const;
    bool isFocused() const;
    std::string getInfoStr() const;
    glm::uvec2 getWindowSize() const;
    glm::uvec2 getFramebufferSize() const;
    glm::uvec2 getPosition() const;
    std::string getTitle() const;
    std::vector<const char *> getGLFWVulkanExtensions() const;

  private:
    GLFWwindow *window_;
};
} // namespace graphics
