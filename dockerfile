FROM unityci/editor:2022.3.0f1-webgl-0.20.0

WORKDIR /workspace

COPY . .

RUN /opt/unity-editor/Unity \
    -quit \
    -batchmode \
    -nographics \
    -projectPath /workspace \
    -buildWebGLPlayer /workspace/Builds/WebGL

EXPOSE 8080

CMD ["python3", "-m", "http.server", "8080", "--directory", "/workspace/Builds/WebGL"]
