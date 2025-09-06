#include "graphics/renderer.hpp"
#include "graphics/window.hpp"
#include "logger.hpp"
#include <string>

int main() {
    logging::Init();
#ifndef NDEBUG
    logging::info("Debug mode active");
#endif
    graphics::WindowSettings settings;
    settings.title = "Opxel";

    graphics::Window window(settings);
    graphics::Renderer renderer(&window);

    while (!window.shouldClose()) {
        window.pollEvents();
    }

    return 0;
}
