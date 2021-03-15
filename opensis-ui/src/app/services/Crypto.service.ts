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
  var keySize = 256;
    let keys = environment.encryptionKey;
    var salt = CryptoJS.lib.WordArray.random(16); var key = CryptoJS.PBKDF2(keys, salt, {
    keySize: keySize/32,
    iterations: 100
    }); var iv = CryptoJS.lib.WordArray.random(128/8);
    var encrypted = CryptoJS.AES.encrypt(value, key, {
    iv: iv,
    padding: CryptoJS.pad.Pkcs7,
    mode: CryptoJS.mode.CBC
    }); var result =CryptoJS.enc.Base64.stringify(salt.concat(iv).concat(encrypted.ciphertext));
   return result;
}

//The get method is use for decrypt the value.
decrypt(value){
  let keys = environment.encryptionKey;
  let iv= environment.encryptioniv;
  

  var bytes  = CryptoJS.AES.decrypt(value.toString(), keys);
  var decryptedData = JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
  return decryptedData;
}

dataEncrypt(value) {

  var key = CryptoJS.enc.Utf8.parse(environment.dataEncryptionKey);
  var iv = CryptoJS.enc.Utf8.parse(environment.dataEncryptionKey)

  var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(value), key,
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
    var key = CryptoJS.enc.Utf8.parse(environment.dataEncryptionKey);
    var iv = CryptoJS.enc.Utf8.parse(environment.dataEncryptionKey);
    var decrypted = CryptoJS.AES.decrypt(value?.toString(), key, {
      keySize: 128 / 8,
      iv: iv,
      mode: CryptoJS.mode.CBC,
      padding: CryptoJS.pad.Pkcs7
    });
    return decrypted.toString(CryptoJS.enc.Utf8);
  }
}
}
