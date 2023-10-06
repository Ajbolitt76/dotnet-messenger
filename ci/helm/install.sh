#!/bin/bash
app_name='messenger'
docker_password=$DOCKER_PASSWORD
docker_username=$DOCKER_USERNAME
container_registry=$DOCKER_REGISTRY
skip_infrastructure=''
namespace='messenger'
registry_suffix=''
clean='yes'
dns=''

while [[ $# -gt 0 ]]; do
  case "$1" in
  -n | --app-name)
    app_name="$2"
    shift 2
    ;;
  -d | --dns)
    dns="$2"
    shift 2
    ;;
  -p | --docker-password)
    docker_password="$2"
    shift 2
    ;;
  -u | --docker-username)
    docker_username="$2"
    shift 2
    ;;
  -r | --registry)
    container_registry="$2"
    shift 2
    ;;
  -rs | --registry-suffix)
    registry_suffix="$2"
    shift 2
    ;;
  --skip-infrastructure)
    skip_infrastructure='yes'
    shift
    ;;
  --namespace)
    namespace="$2"
    shift 2
    ;;
  --no-clean)
    clean=''
    ;;
  *)
    echo "Unknown option $1"
    exit 2
    ;;
  esac
done

if [[ -n $container_registry ]] && [[ -z $docker_password ]]; then
  read -p "Enter password: " -s -r docker_password 
fi;

use_custom_registry=''

if [[ -n $container_registry ]]; then
  echo "################ Using custom registry $container_registry ##################"
  use_custom_registry='yes'
fi

if [[ -z $dns ]]; then
  echo "No DNS specified. Ingress resources will be bound to public IP."
fi

if [[ $clean ]]; then
  echo "Cleaning previous helm releases..."
  if [[ -z $(helm ls -q --namespace $namespace | grep -v "^manual") ]]; then
    echo "No previous releases found"
  else
    helm --namespace $namespace uninstall $(helm ls -q --namespace $namespace | grep -v "^manual")
    echo "Previous releases deleted"
    waitsecs=10; while [ $waitsecs -gt 0 ]; do echo -ne "...$waitsecs\033[0K\r"; sleep 1; : $((waitsecs--)); done
  fi
fi

# $1 - name
# $2 - additional args
function helmInstall() {
  echo "Installing: $1"
  # shellcheck disable=SC2086
  if [[ $use_custom_registry ]]; then 
    helm install "$app_name-$1" $1 \
      --namespace "$namespace" \
      --create-namespace \
      --set app.name=$app_name \
      --set inf.k8s.dns=$dns \
      --set inf.registry.secretName=docker-secret \
      --set inf.registry.suffix=$registry_suffix \
      --set inf.registry.server=$container_registry \
      --set inf.registry.login=$docker_username \
      --set inf.registry.pwd=$docker_password \
      $2
  else
    helm install "$app_name-$1" $1 \
      --namespace "$namespace" \
      --create-namespace \
      --set app.name=$app_name \
      --set inf.k8s.dns=$dns \
      $2
  fi 
}

echo "#################### Begin $app_name installation using Helm ####################"

if [[ ! $skip_infrastructure ]]; then
  echo "################ Installing infrastructure ################"
  helmInstall "messenger-common"
  helmInstall "messenger-db"
  helmInstall "messenger-redis"
fi

charts=(meesenger-backend meesenger-frontend)

echo "################ Installing charts ################"
for chart in "${charts[@]}"
do
    echo "Installing: $chart"
    helmInstall "$chart"
done