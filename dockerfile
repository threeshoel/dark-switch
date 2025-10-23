# Stage 1: Build the Unity project
# We use a special image from GameCI that has the Unity Editor installed
FROM unityci/editor:ubuntu-2022.3.26f1-linux-il2cpp-3.2.0 AS builder

# NEW: Accept the license content as a build argument from Jenkins
ARG UNITY_LICENSE_CONTENT

# NEW: Activate the Unity license
RUN mkdir -p /root/.local/share/unity3d/Unity && echo "$UNITY_LICENSE_CONTENT" > /root/.local/share/unity3d/Unity/Unity_lic.ulf
# Set the working directory inside the container
WORKDIR /workspace

# Copy your entire project from Jenkins into the container
COPY . .

# Build the Unity project for WebGL
# THIS IS THE COMMAND YOU MUST FIX:
# Replace 'YOUR_PROJECT_FOLDER' with the real folder name
# (e.g., 'dark-switch-game', or whatever contains Assets/)
RUN unity-editor \
    -projectPath /workspace/dark-switch\
    -buildTarget WebGL \
    -quit -batchmode -nographics \
    -logFile /workspace/unity_build.log\
    || (cat /workspace/unity_build.log && exit 1)


# ---
# Stage 2: Serve the game with a lightweight web server
FROM nginx:alpine

# Copy the *build output* (from Stage 1) into the web server's public folder
# YOU MUST ALSO FIX 'YOUR_PROJECT_FOLDER' HERE:
COPY --from=builder /workspace/dark-switch/Builds/WebGL /usr/share/nginx/html

# Tell Docker that the container will listen on port 80 (Nginx's default)
EXPOSE 80

# The command to start the Nginx server
CMD ["nginx", "-g", "daemon off;"]