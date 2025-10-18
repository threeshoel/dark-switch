pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/Kaushik-Bommu/dark-switch.git'
            }
        }

        stage('Build with Docker') {
            steps {
                sh 'docker build -t unity-platformer .'
            }
        }

        stage('Cleanup Previous Container') {
            steps {
                sh 'docker rm -f unity-webgl || true'
            }
        }

        stage('Run Container') {
            steps {
                sh 'docker run -d -p 8080:8080 --name unity-webgl unity-platformer'
            }
        }
    }
}
