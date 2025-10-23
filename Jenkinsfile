pipeline {
    agent any

    environment {
        UNITY_LICENSE_CONTENT = credentials('UNITY_LICENSE_CONTENT')
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
                sh 'docker build --pull -t $DOCKER_IMAGE .'
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
                sh 'docker run -d -p 8080:8080 --name $CONTAINER_NAME $DOCKER_IMAGE'
            }
        }

        stage('Post Build Info') {
            steps {
                echo "Game deployed at http://<Jenkins-Server-IP>:8080"
            }
        }
    }

    post {
        failure {
            echo "Build or deployment failed! Check logs."
        }
    }
}
