docker run --rm -it -w /export -v ${PWD}:/export --entrypoint /bin/ash frapsoft/openssl ./cert.sh

Move-Item -Path .\caracal-*.* -Destination .\caracal -Force