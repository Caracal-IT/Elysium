# TLS Certs
This project will create the self singed tls certificates, register it as a trusted certificate and add
the dev.divigraph.com to the host file.

## Steps

## command
1. Run powershell in "Administrator" mode.
2. Navigate to the ~/build/certs folder.
3. Run the install.ps1 file.

   ```powershell
    ./install.ps1
   ```
4. Click on the windows icon in the tool bar at the bottom of the screen.
5. Enter cert and click on "Manage user certificates".
6. Check if the "dev.divigraph.com" certificate is in the "Trusted Root Certificate Authorities" store.