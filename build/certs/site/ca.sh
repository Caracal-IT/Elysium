PWD=`openssl rand -base64 32`
echo "$PWD" > abc.pwd

#
# CREATE CA KEY
#
echo ---------------------------------------------------
openssl genpkey -algorithm RSA -out ca.key

#
# CREATE CA CERT
#
echo ---------------------------------------------------
openssl req -x509 -new -nodes -key ca.key -sha256 -days 3650 -config ca.cnf -out ca.crt

#
# CREATE CA VERIFY
#
echo ---------------------------------------------------
#openssl x509 -in ca.crt -noout -text