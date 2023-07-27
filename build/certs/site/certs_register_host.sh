if ! grep -q -E '^\s*127.0.0.1\s+dev.caracal.com(\s|$)' /etc/hosts; then
    sudo sh -c "echo '127.0.0.1 dev.caracal.com' >> /etc/hosts"
    echo "Entry added"
else
    echo "Entry already exists"
fi

sudo killall -HUP mDNSResponder

#