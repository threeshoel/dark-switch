# Base image from Unity CI (you can change the Unity version)
FROM unityci/editor:2022.3.0f1-windows-mono-1.0.0

# Set working directory inside container
WORKDIR /workspace

# Copy your Unity project files into the container
COPY . .

# Build the Unity project for WebGL or Windows
# WebGL example (change to your target)
RUN unity-editor \
    -batchmode \
    -nographics \
    -quit \
    -projectPath /workspace \
    -buildWebGLPlayer /workspace/Builds/WebGL/

# Expose a port (if you plan to run a local web server)
EXPOSE 8080

# Simple command to serve the WebGL build
CMD ["python3", "-m", "http.server", "8080", "--directory", "/workspace/Builds/WebGL"]
