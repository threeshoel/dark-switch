FROM unityci/editor:2022.3.0f1-windows-mono-1.0.0
WORKDIR /workspace
COPY . .
RUN /opt/unity/Editor/Unity -batchmode -nographics -quit -projectPath . -buildWindows64Player Build/MyGame.exe
