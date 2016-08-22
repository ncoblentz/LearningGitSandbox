#!/bin/bash
ln -s /etc/ssl/certs/ca.cert.pem /etc/ssl/certs/`openssl x509 -hash -in /etc/ssl/certs/ca.cert.pem -noout 2>/dev/null`.0
cat /etc/ssl/certs/ca.cert.pem >> /etc/ssl/certs/ca-certificates.crt