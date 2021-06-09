#include<iostream>
#include<glad/glad.h>
#include<GLFW/glfw3.h>

int main() {
	// Initialize GLFW
	glfwInit();
	// Tell GLFW what version of OpenGl we are using
	// In this case we are using OpenGL 3.3
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	// Tell GLFW we are using th CORE profile
	// So that means we only have the modern functions
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

	// Create a GLFWwindow object of 800 by 800 pixel
	GLFWwindow* window = glfwCreateWindow(800, 800, "YoutubeGL", NULL, NULL);
	// Error check if the windwo fails to create
	if (window == NULL) {
		std::cout << "Failed to create GLFW window";
		glfwTerminate();
		return -1;
	}
	// Introduce the window into the current context
	glfwMakeContextCurrent(window);

	// Load GLAD so it configures OpenGL
	gladLoadGL();

	// Specify the viewport of OpenGl in the Window
	glViewport(0, 0, 800, 800);

	// Specify the background color
	glClearColor(0.07, 0.13f, 0.17f, 1);

	// Clean the back buffer and afssign the new color to it
	glClear(GL_COLOR_BUFFER_BIT);
	// Swap back buffer with the front buffer
	glfwSwapBuffers(window);

	// main loop
	while (!glfwWindowShouldClose(window)) {
		// Take care of all GLFW elements
		glfwPollEvents();
	}
	// Delete the window before ending the program
	glfwDestroyWindow(window);
	// Terminate GLFW before ending the program
	glfwTerminate();
	return 0;
}