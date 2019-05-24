pipeline {

agent {
    kubernetes {
      cloud 'eks-swimlane-io'
      label "jenkins-k8s-${UUID.randomUUID().toString()}"
      yaml """
kind: Pod
metadata:
  name: jenkins-k8s
spec:
  securityContext:
    runAsUser: 1001
  containers:
  - name: jnlp
    image: 'jenkins/jnlp-slave:latest'
  - name: jenkins-linux-slave
    image: 'nexus.swimlane.io:5000/jenkins-linux-slave:do-917-add-dotnet-3'
    command: ["tail", "-f", "/dev/null"]
    resources:
      requests:
        memory: "2000Mi"
        cpu: "900m"
      limits:
        memory: "2000Mi"
        cpu: "1200m"
  imagePullSecrets:
  - name: swimlane-nexus
"""
    }
  }

  options {
    timeout(time: 15, unit: 'MINUTES')
    timestamps()
    buildDiscarder(logRotator(artifactNumToKeepStr: '1'))
  }

  environment {
    Newtonsoft = credentials('newtonsoft-jsonschema-license')
  }

  stages{
    stage('Dotnet Restore') {
      steps {
        container("jenkins-linux-slave") {
            sh("dotnet restore QSwag.sln")
        }
      }
    }
    stage('Build') {
      steps {
        container("jenkins-linux-slave") {
            sh("dotnet build -c Release QSwag.sln")
        }
      }
    }
    stage('Run Tests') {
      steps {
          container("jenkins-linux-slave") {
            dir('src/QSwagTest') {
              sh('dotnet test')
             }
          }
      }
    }
    stage('Publish') {
      /*when {
        anyOf {
          branch 'master'
        }
      }*/
      steps {
          container("jenkins-linux-slave") {
            withCredentials([string(credentialsId: 'nuget-token',  variable: 'nugetToken')]) {
              dir('src/QSwagGenerator') {  
                sh('dotnet pack -c Release')
                sh('dotnet nuget push bin/Release/*.nupkg -s https://api.nuget.org/v3/index.json -k $nugetToken')
              }
                dir('src/QSwagSchema') {  
                sh('dotnet pack -c Release')
                sh('dotnet nuget push bin/Release/*.nupkg -s https://api.nuget.org/v3/index.json -k $nugetToken')
              }
            }
          }
      }
    }
  }

  post {
    always {
      cleanWs()
    }
    failure {
      script {
        if (currentBuild.getPreviousBuild() &&
            currentBuild.getPreviousBuild().getResult().toString() == "SUCCESS") {
          slackSend(
            baseUrl: 'https://swimlane.slack.com/services/hooks/jenkins-ci/',
            botUser: true,
            channel: '#platform_notification',
            color: 'red',
            message: "QSwag artifact build on branch ${env.GIT_BRANCH} failed and is now RED: <${env.RUN_DISPLAY_URL}|Build #${env.BUILD_NUMBER}> ",
            teamDomain: 'swimlane',
            tokenCredentialId: 'slack-token')
        }
      }
    }
    success {
      // Post successful notification when failing branch has been fixed
      script {
       if (currentBuild.getPreviousBuild() &&
           currentBuild.getPreviousBuild().getResult().toString() != "SUCCESS") {
           slackSend(
             baseUrl: 'https://swimlane.slack.com/services/hooks/jenkins-ci/',
             botUser: true,
             channel: '#platform_notification',
             color: 'good',
             message: "QSwag artifact build on branch ${env.GIT_BRANCH} passed and is now GREEN: <${env.RUN_DISPLAY_URL}|Build #${env.BUILD_NUMBER}> ",
             teamDomain: 'swimlane',
             tokenCredentialId: 'slack-token')
         }
      }
    }
  }
}
