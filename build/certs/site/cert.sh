name='caracal-dev'
cnf='cert.cnf'
PWD='password1'

#PWD=`openssl rand -base64 32`
#echo "$PWD" > $name.pwd


#
# CREATE THE CERTIFICATE SIGNING REQUEST (CSR)
#
echo ----------------------------------------------------
openssl req                                             \
    -new                                                \
    -x509                                               \
    -nodes                                              \
    -days 365                                           \
    -newkey rsa:2048                                    \
    -keyout $name.key                                   \
    -out $name.crt                                      \
    -config $cnf                                        \
    -passin pass:$PWD                                   \
    -passout pass:$PWD

#
# CONVERT TO PFX
#
echo ---------------------------------------------------
openssl pkcs12                                          \
    -inkey $name.key                                    \
    -in $name.crt                                       \
    -export -out $name.pfx                              \
    -passin pass:$PWD                                   \
    -passout pass:$PWD
    
#
# CREATE PEM
#   
echo ---------------------------------------------------
openssl pkcs12 -in $name.pfx -nocerts -nodes -out $name.rsa -passin pass:$PWD 
openssl pkcs12 -in $name.pfx -out $name.pem -nodes -passin pass:$PWD