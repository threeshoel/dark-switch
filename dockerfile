# Use a valid UnityCI image with WebGL support
FROM unityci/editor:ubuntu-2022.3.26f1-linux-il2cpp-3.2.0 AS builder

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
    -buildWebGLPlayer /workspace/Builds/WebGL \
    -logFile /workspace/unity_build.log

# Stage 2: Serve with Nginx
FROM nginx:alpine
COPY --from=builder /workspace/Builds/WebGL /usr/share/nginx/html
EXPOSE 8080
CMD ["nginx", "-g", "daemon off;"]
