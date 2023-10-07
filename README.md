# BingImageCreator.Net

BingImageCreator.Net is Bing Image Creator cli tool inspired by https://github.com/acheong08/BingImageCreator repo implemented on the .Net platform.

```
usage: TODO
```

## Getting authentication
### Chromium based browsers (Edge, Opera, Vivaldi, Brave)
- Go to https://bing.com/.
- F12 to open console
- In the JavaScript console, type `cookieStore.get("_U").then(result => console.log(result.value))` and press enter
- Copy the output. This is used in `--U` or `auth_cookie`.

### Firefox
- Go to https://bing.com/.
- F12 to open developer tools
- navigate to the storage tab
- expand the cookies tab
- click on the `https://bing.com` cookie
- copy the value from the `_U`