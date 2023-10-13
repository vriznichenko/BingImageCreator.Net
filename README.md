# BingImageCreator.Net

BingImageCreator.Net is Bing Image Creator cli tool inspired by https://github.com/acheong08/BingImageCreator repo implemented on the .Net platform.

```usage: create config.json file in the same directory as sample_config.json. Then edit config json with next rules:```

| Field         | Type     | Comment |
|--------------|-----------|------------|
| input  | object | object that stores input preferences |
| input.prompt | string | string with description of desired Image Creator result |
| input.base_url  | string | this is base Bing url, use 'https://www.bing.com' |
| input.polling_max_retries  | int | this value defines how many requests will polling request do (awaiting for Image Creator result). Interval between requests - 1s. If you have special coins 100 is enough, change this value only if you haven't coins|
| input.regexp  | object | object for defining regex pattern |
| input.regexp.pattern  | string | regex search pattern. Use 'src=\"([^\"]+)\"' |
| input.headers  | object | object for defining header key value pairs |
| input.header.name  | string | header name. If there are no problems use default from sample_config.json |
| input.header.value  | string | header value. If there are no problems use default from sample_config.json |
| input.cookies  | object | object for defining cookie key value pairs |
| input.cookie.name  | string | cookie name. Required cookies: '_U': see how to get in topic below |
| input.cookie.value  | string | header value. Required cookies: '_U': see how to get in topic below |
| output  | object | object that stores output preferences |
| output.output_dir | string | output directory path |
| output.temp_dir | string | temp directory path |

## Getting authentication
### Chromium based browsers (Edge, Opera, Vivaldi, Brave)
- Go to https://bing.com/.
- F12 to open console
- In the JavaScript console, type `cookieStore.get("_U").then(result => console.log(result.value))` and press enter

### Firefox
- Go to https://bing.com/.
- F12 to open developer tools
- navigate to the storage tab
- expand the cookies tab
- click on the `https://bing.com` cookie
- copy the value from the `_U`
