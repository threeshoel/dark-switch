# Use a valid Linux Unity editor image from UnityCI
FROM unityci/editor:ubuntu-2021.3.57f2-windows-mono-3.2.0

# Set working directory inside container
WORKDIR /workspace

# Copy your Unity project into container
COPY . .

# Build Unity project for WebGL
RUN /opt/unity-editor/Unity \
    -quit \
    -batchmode \
    -nographics \
    -projectPath /workspace \
    -buildWebGLPlayer /workspace/Builds/WebGL

# Expose port to view WebGL build
EXPOSE 8080

# Serve the WebGL build via Python HTTP server
CMD ["python3", "-m", "http.server", "8080", "--directory", "/workspace/Builds/WebGL"]
