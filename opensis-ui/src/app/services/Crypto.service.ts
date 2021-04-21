import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CryptoService {

constructor() { }



//The set method is use for encrypt the value.
encrypt(value){
  const keySize = 256;
    const keys = environment.encryptionKey;
    const salt = CryptoJS.lib.WordArray.random(16); const key = CryptoJS.PBKDF2(keys, salt, {
    keySize: keySize/32,
    iterations: 100
    }); const iv = CryptoJS.lib.WordArray.random(128/8);
    let encrypted = CryptoJS.AES.encrypt(value, key, {
    iv: iv,
    padding: CryptoJS.pad.Pkcs7,
    mode: CryptoJS.mode.CBC
    }); const result =CryptoJS.enc.Base64.stringify(salt.concat(iv).concat(encrypted.ciphertext));
   return result;
}

//The get method is use for decrypt the value.
decrypt(value){
  const keys = environment.encryptionKey;
  const iv= environment.encryptioniv;
  

  const bytes  = CryptoJS.AES.decrypt(value.toString(), keys);
  const decryptedData = JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
  return decryptedData;
}

dataEncrypt(value) {

  const key = CryptoJS.enc.Utf8.parse(environment.dataEncryptionKey);
  const iv = CryptoJS.enc.Utf8.parse(environment.dataEncryptionKey)

  const encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value), key,
    {
      keySize: 128 / 8,
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });
  return encrypted.toString();
}

dataDecrypt(value) {
  if(value){
    const key = CryptoJS.enc.Utf8.parse(environment.dataEncryptionKey);
    const iv = CryptoJS.enc.Utf8.parse(environment.dataEncryptionKey);
    const decrypted = CryptoJS.AES.decrypt(value?.toString(), key, {
      keySize: 128 / 8,
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });
    return decrypted.toString(CryptoJS.enc.Utf8);
  }
}
}
