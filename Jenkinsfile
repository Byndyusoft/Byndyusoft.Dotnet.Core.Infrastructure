pipeline {
    agent { label 'master' }
    environment {
        DOCKER_REGISTRY = 'local'
        DOCKER_IMAGE  = 'app-bs'
		contextpath = '.'
		projectdll = 'Byndyusoft.Dotnet.Core.Samples.Web.Application.dll'
		projectpath = '/app/src/Web.Application'
		testspath = '/app/tests/Tests'
		arg1 = '--xml_docs'
		arg2 = 'Byndyusoft.Dotnet.Core.Samples.Web.Application.xml' 
        dockerservice = 'app' // check this also in docker-compose file
		grafanapass = 'admin'
    }

    stages {
        
        stage('Build containers') {   
            environment {
                CONTAINER_TAG = "${env.GIT_COMMIT}"
            }
            steps {              
                 sh 'printenv'
                 script { 
                        if (env.GIT_BRANCH == 'origin/master') {
                            echo 'Building from branch '+ env.GIT_BRANCH
                            sh "docker-compose build"
//                            sh "docker-compose push"
                        }
                        if (env.GIT_BRANCH == 'release') {
                            input 'Are you sure to deploy to Production?'
                            echo 'Building from branch '+ env.GIT_BRANCH
                            sh "docker pull ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${CONTAINER_TAG}"
                            sh "docker tag ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${CONTAINER_TAG} ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:prod"
                            sh "docker push ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:prod"
                            sh "docker-compose push"
                        }
                  }
            }
        }
        stage ('Test') {
            environment {
                CONTAINER_TAG = "${env.GIT_COMMIT}"
            }
            steps {               
 //                sh "docker pull ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${CONTAINER_TAG}"                    
                 sh "id=\$(docker create ${DOCKER_REGISTRY}/${DOCKER_IMAGE}:${CONTAINER_TAG}) && docker cp \$id:/app/unit_tests.xml  unit_tests.xml && docker rm -v \$id"
                 step([$class: 'MSTestPublisher', testResultsFile:"**/*.xml", failOnError: true, keepLongStdio: true])
                  }
        }

//        stage ('Migrations') {
//            agent {
//                docker {
//                    image 'microsoft/dotnet:2.1-sdk'
//                    args '--add-host=postgresql_stg:172.17.0.1'
//                }
//            }
//            environment {
//                ENVIRONMENT = 'Staging'
//                CONTAINER_TAG = "${env.GIT_COMMIT}"
//            }
//            steps { 
//                script {
//                    sh 'cd src/Migrations && dotnet restore && dotnet  publish -o migration_runner'
//					if (env.GIT_BRANCH == 'origin/master') {
//						def statusCode = sh script: 'cd src/Migrations/migration_runner && dotnet ByndyuSoft.Migrations.dll --environment Staging', returnStatus: true
//						if ( statusCode != 0 ) {
//										echo "Migration Errors found!" 
//										echo "${statusCode}"
//										currentBuild.result = 'FAILED'										
//										sh "exit ${statusCode}"
//										}
//						}
//					if (env.GIT_BRANCH == 'release') {
//						def statusCode = sh script: 'cd src/Migrations/migration_runner && dotnet ByndyuSoft.Migrations.dll --environment Production', returnStatus: true
//						if ( statusCode != 0 ) {
//										echo "Migration Errors found!" 
//										echo "${statusCode}"
//										currentBuild.result = 'FAILED'
//										sh "exit ${statusCode}"
//										}
//						}                    
//
//                    }
//                  }
//        }

        stage('Staging') {
            environment {
                ENVIRONMENT = 'Staging'
                CONTAINER_TAG = "${env.GIT_COMMIT}"
                NODEROLE = "manager"
                }          
            steps {
                script {
                    if (env.GIT_BRANCH == 'origin/master') {
                        sh "docker stack deploy --with-registry-auth --compose-file docker-compose.yml bsapp"
                        sh "curl --user admin:${grafanapass} 'http://grafana:3000/api/datasources' -X POST -H 'Content-Type: application/json;charset=UTF-8' --data-binary '{\"name\":\"elasticsearch-bsapp-${dockerservice}\",\"isDefault\":false ,\"type\":\"elasticsearch\",\"url\":\"http://elasticsearch:9200\",\"access\":\"proxy\",\"basicAuth\":false,\"database\":\"[bsapp_${dockerservice}-]YYYY.MM.DD\",\"jsonData\": {\"esVersion\": 5, \"interval\": \"Daily\", \"timeField\": \"@timestamp\"}}'"
                        }
               }
            }
        }

        stage('Production') {
            environment {
                ENVIRONMENT = 'Production'
                CONTAINER_TAG = "prod"
                NODEROLE = "worker"
                }          
             when { branch 'release' }
            steps {
                input message: "Deploy to production?", ok: "Yes"
                script {
                    if (env.GIT_BRANCH == 'release') {
                        sh "docker stack deploy --with-registry-auth --compose-file docker-compose.yml bsprd"
                        }
                  }
            }
        }
    }
}