# Stage 1: Build Unity WebGL
FROM unityci/editor:ubuntu-2021.3.57f2-webgl-0.16.0 AS builder

# Set working directory
WORKDIR /workspace

# Copy Unity project into container
COPY . .

# Build WebGL project
RUN /opt/unity-editor/Unity \
    -quit \
    -batchmode \
    -nographics \
    -projectPath /workspace \
    -buildWebGLPlayer /workspace/Builds/WebGL \
    -logFile /workspace/unity_build.log

# Stage 2: Serve with Nginx
FROM nginx:alpine

# Copy WebGL build from previous stage
COPY --from=builder /workspace/Builds/WebGL /usr/share/nginx/html

# Expose port
EXPOSE 8080

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
