# Table of Contents
1. [Added ERB Instructions](#added-instructions)
    - [PRINT_HTMLSL](#print_htmlsl)
    - [CHKFONTEDGE](#chkfontedge)
    - [FONTEDGE_COLOR](#fontedge_color)
    - [RESET_FONTEDGE_COLOR](#reset_fontedge_color)
    - [GETFONTEDGECOLOR](#getfontedgecolor)
    - [GETDEFFONTEDGECOLOR](#getdeffontedgecolor)
2. [Added EmuEra functionality](#added-emuera-functionality)
    - [Custom window icon](#custom-icon-for-taskbarmain-view-icon)

# Added Instructions
These are the instructions we've added that Japan doesn't have:
## PRINT_HTMLSL
PRINT_HTML, but without breaking the line.

Example usage:
```erb
HTML_PRINTSL @"<nonbutton title='Stamina, probably.'>STA </nonbutton>"
BAR 80, 100, 10
PRINTL This text should be on the same line
HTML_PRINTSL @"<nonbutton title='Energy, probably.'>ENE </nonbutton>"
BAR 60, 100, 10
PRINTL This text should be on the same line
```
This results in:
```
STA [********..]This text should be on the same line
ENE [******....]This text should be on the same line
```

## CHKFONTEDGE
Returns 0 or 1, depending on whether FONTEDGE is enabled.

Example: 
```erb
CHKFONTEDGE
IF RESULT:0
    PRINT font is edgy
ENDIF
```
## FONTEDGE_COLOR
Sets the font edge color.

Ex: `FONTEDGE_COLOR 255, 0, 255`
## RESET\_FONTEDGE\_COLOR
Sets the font edge color to default. Takes no arguments.
## GETFONTEDGECOLOR
Returns font edge color. Behaves similarly to other GET commands related to color.
## GETDEFFONTEDGECOLOR
Returns default font edge color.

# Added EmuEra functionality
## Custom icon for taskbar/main view icon
This is a hidden config item that you can use to set the icon that appears for the game window on launch. When you start Anchor it'll be added automatically at the end of the config file. This is what it looks like in action, though (custom icon is an anchor):

![Custom icon example](/MarkdownContent/CustomIconExample.png)

Default value: `Anchor- Path to a custom window icon (PNG):`

Valid config values:
- Path relative to EXE: `Anchor- Path to a custom window icon (PNG): icon.png`
- Absolute path (probably don't do this if you're a dev): `Anchor- Path to a custom window icon (PNG): C:\NOTLEWD\OKAYACTUALLY\ITSLEWD\icon.png`