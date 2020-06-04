# Tetris

<a href="https://github.com/VsIG-official/ConsoleGameCSharp/blob/Another-One/Images/1.png"><img src="https://github.com/VsIG-official/ConsoleGameCSharp/blob/Another-One/Images/1.png" title="VsIG" alt="VsIG"></a>

## Table of Contents (Optional)

- [Description](#description)
- [Badges](#badges)
- [Contributing](#contributing)
- [License](#license)

### Description

> Hi, My dear Friend! In this repo You'll see console tetris game! Why tetris? Well, I decided to make a tetris because I love tetris! In this project I use C# and Visual Studio. Hope You will enjoy!

## Badges

[![Build Status](http://img.shields.io/travis/badges/badgerbadgerbadger.svg?style=flat-square)](https://travis-ci.org/badges/badgerbadgerbadger) 

> GIF Tools

- Use <a href="http://recordit.co/" target="_blank">**Recordit**</a> to create quicks screencasts of your desktop and export them as `GIF`s.
- For terminal sessions, there's <a href="https://github.com/chjj/ttystudio" target="_blank">**ttystudio**</a> which also supports exporting `GIF`s.

**Recordit**

![Recordit GIF](http://g.recordit.co/iLN6A0vSD8.gif)

**ttystudio**

![ttystudio GIF](https://raw.githubusercontent.com/chjj/ttystudio/master/img/example.gif)


---

## Example

```C#

public void MoveRight(ref char[][] tetrisGrid, char shapes, char freeSpace)
		{
			for (int i = 0; i < matrixWidth; i++)
			{
				for (int j = matrixHeight - 1; j >= 0; j--)
				{
					if (tetrisGrid[i][j] == shapes && tetrisGrid[i][j + 1] == freeSpace)
					{
						char tempMatrix = tetrisGrid[i][j];
						tetrisGrid[i][j] = tetrisGrid[i][j + 1];
						tetrisGrid[i][j + 1] = tempMatrix;
					}
				}
			}
		}
```

---

## Contributing

> To get started...

### Step 1

- **Option 1**
    - 🍴 Fork this repo!

- **Option 2**
    - 👯 Clone this repo to your local machine using `https://github.com/joanaz/HireDot2.git`

### Step 2

- **HACK AWAY!** 🔨🔨🔨

### Step 3

- 🔃 Create a new pull request using <a href="https://github.com/joanaz/HireDot2/compare/" target="_blank">`https://github.com/joanaz/HireDot2/compare/`</a>.

---

## Team

> Or Contributors/People

| <a href="https://github.com/VsIG-official" target="_blank">**VsIG**</a> | <a href="https://github.com/VsIG-official" target="_blank">**VsIG**</a> | <a href="https://github.com/VsIG-official" target="_blank">**VsIG**</a> |
| :---: |:---:| :---:|
| [![VsIG](https://avatars0.githubusercontent.com/u/50269023?s=400&u=522283a8fce57866b73427f94a742fb83e0b1b40&v=4)](https://github.com/VsIG-official)    | [![VsIG](https://avatars0.githubusercontent.com/u/50269023?s=400&u=522283a8fce57866b73427f94a742fb83e0b1b40&v=4)](https://github.com/VsIG-official) | [![VsIG](https://avatars0.githubusercontent.com/u/50269023?s=400&u=522283a8fce57866b73427f94a742fb83e0b1b40&v=4)](https://github.com/VsIG-official)  |
| <a href="https://github.com/VsIG-official" target="_blank">`github.com/VsIG-official`</a> | <a href="https://github.com/VsIG-official" target="_blank">`github.com/VsIG-official`</a> | <a href="https://github.com/VsIG-official" target="_blank">`github.com/VsIG-official`</a> |

- You can just grab their GitHub profile image URL
- You should probably resize their picture using `?s=200` at the end of the image URL.

---

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)

- **[MIT license](http://opensource.org/licenses/mit-license.php)**
- Copyright 2020 © <a href="https://github.com/VsIG-official" target="_blank">VsIG</a>.
