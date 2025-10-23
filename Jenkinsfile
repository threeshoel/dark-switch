pipeline {
    agent any

    environment {
        DOCKER_IMAGE = "unity-platformer"
        CONTAINER_NAME = "unity-webgl"
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/Kaushik-Bommu/dark-switch.git'
            }
        }

        stage('Build Docker Image') {
            steps {
                // This is the correct, secure way to pass the license
                withCredentials([string(credentialsId: 'UNITY_LICENSE_CONTENT', variable: 'UNITY_LICENSE')]) {
                    sh 'docker build --pull -t $DOCKER_IMAGE --build-arg UNITY_LICENSE_CONTENT="$UNITY_LICENSE" .'
                }
            }
        }

        stage('Stop Previous Container') {
            steps {
                sh '''
                if [ $(docker ps -q -f name=$CONTAINER_NAME) ]; then
                    docker rm -f $CONTAINER_NAME
                else
                    echo "No existing container running"
                fi
                '''
            }
        }

        stage('Run Container') {
            steps {
                // FIXED: The port is now 8080 (your server) -> 80 (the container)
                sh 'docker run -d -p 8080:80 --name $CONTAINER_NAME $DOCKER_IMAGE'
            }
        }

        stage('Post Build Info') {
            steps {
                echo "Game deployed! Access it at http://<Your-Jenkins-Server-IP>:8080"
            }
        }
    }

    post {
        failure {
            echo "Build or deployment failed! Check logs."
        }
    }
}