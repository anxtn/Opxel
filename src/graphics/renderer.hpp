#include "debugMessenger.hpp"
#include "instance.hpp"
#include <memory>

namespace graphics {
class Renderer {
  public:
    Renderer(graphics::Window *window);
    ~Renderer();

  private:
    std::unique_ptr<Instance> instance_;
    std::unique_ptr<DebugMessenger> debugMessenger_;
};
} // namespace graphics
