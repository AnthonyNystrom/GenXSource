package gui;

import javax.microedition.lcdui.*;
import java.util.*;

public class GUIText extends GUIControl
{
    public boolean useAutoscroll;
    public Font font;
    private boolean multiline;
    private boolean editable;
    public boolean isPassword;
    private char[] textPassword;
    public char[] text;
    public int textLen;
    public int anchor;
    private Vector stringLengths;
    private int nonpressedFrames;
    private int tempCharIndex;
    private char tempChar;
    private int tempButton;
    private int cursorFlashFrames;
    static public boolean isCapital;
    private int cursorPos;
    private int drawOffset;
    private int drawLength;
    private static Hashtable keypad;
//#ifdef BLACKBERRY
    //#ifdef _8120
//#     static final char chars[][] =
//#     {
//#         {
//#             '!', 'q', 'w'
//#         },
//#         {
//#             '1', 'e', 'r'
//#         },
//#         {
//#             '2', 't', 'y'
//#         },
//#         {
//#             '3', 'u', 'i'
//#         },
//#         {
//#             '.', 'o', 'p'
//#         },
//#         {
//#             '?', 'a', 's'
//#         },
//#         {
//#             '4', 'd', 'f'
//#         },
//#         {
//#             '5', 'g', 'h'
//#         },
//#         {
//#             '6', 'j', 'k'
//#         },
//#         {
//#             ',', 'l'
//#         },
//#         {
//#             '@', 'z', 'x'
//#         },
//#         {
//#             '7', 'c', 'v'
//#         },
//#         {
//#             '8', 'b', 'n'
//#         },
//#         {
//#             '9', 'm'
//#         },
//#         {
//#             '0', ' '
//#         }
//#     };
    //#elifdef _8320
//#         static final char chars[][] =
//#         {
//#             {
//#                 '#', 'q'
//#             },
//#             {
//#                 '1', 'w'
//#             },
//#             {
//#                 '2', 'e'
//#             },
//#             {
//#                 '3', 'r'
//#             },
//#             {
//#                 '(', 't'
//#             },
//#             {
//#                 ')', 'y'
//#             },
//#             {
//#                 '_', 'u'
//#             },
//#             {
//#                 '-', 'i'
//#             },
//#             {
//#                 '+', 'o'
//#             },
//#             {
//#                 '@', 'p'
//#             },
//#             {
//#                 '*', 'a'
//#             },
//#             {
//#                 '4', 's'
//#             },
//#             {
//#                 '5', 'd'
//#             },
//#             {
//#                 '6', 'f'
//#             },
//#             {
//#                 '/', 'g'
//#             },
//#             {
//#                 ':', 'h'
//#             },
//#             {
//#                 ';', 'j'
//#             },
//#             {
//#                 '\'', 'k'
//#             },
//#             {
//#                 '"', 'l'
//#             },
//#             {
//#                 '7', 'z'
//#             },
//#             {
//#                 '8', 'x'
//#             },
//#             {
//#                 '9', 'c'
//#             },
//#             {
//#                 '?', 'v'
//#             },
//#             {
//#                 '!', 'b'
//#             },
//#             {
//#                 ',', 'n'
//#             },
//#             {
//#                 '.', 'm'
//#             },
//#             {
//#                 ' ', ' '
//#             }
//#         };
//# 
    //#endif
//#else
    static final char chars[][] =
    {
        {
            '.', ',', '0'
        },
        {
            ' ', '@', '1'
        },
        {
            'a', 'b', 'c', '2'
        },
        {
            'd', 'e', 'f', '3'
        },
        {
            'g', 'h', 'i', '4'
        },
        {
            'j', 'k', 'l', '5'
        },
        {
            'm', 'n', 'o', '6'
        },
        {
            'p', 'q', 'r', 's', '7'
        },
        {
            't', 'u', 'v', '8'
        },
        {
            'w', 'x', 'y', 'z', '9'
        }
    };
//#endif
    public GUIText(Font font, boolean multiline, boolean editable)
    {
        super();

//#ifdef BLACKBERRY
//#         if (keypad == null)
//#         {
//#             keypad = new Hashtable();
            //#ifdef _8120
//#             keypad.put(new Integer(App.Const.KEY_Q), new Integer(0));
//#             keypad.put(new Integer(App.Const.KEY_E), new Integer(1));
//#             keypad.put(new Integer(App.Const.KEY_T), new Integer(2));
//#             keypad.put(new Integer(App.Const.KEY_U), new Integer(3));
//#             keypad.put(new Integer(App.Const.KEY_O), new Integer(4));
//#             keypad.put(new Integer(App.Const.KEY_A), new Integer(5));
//#             keypad.put(new Integer(App.Const.KEY_D), new Integer(6));
//#             keypad.put(new Integer(App.Const.KEY_G), new Integer(7));
//#             keypad.put(new Integer(App.Const.KEY_J), new Integer(8));
//#             keypad.put(new Integer(App.Const.KEY_L), new Integer(9));
//#             keypad.put(new Integer(App.Const.KEY_Z), new Integer(10));
//#             keypad.put(new Integer(App.Const.KEY_C), new Integer(11));
//#             keypad.put(new Integer(App.Const.KEY_B), new Integer(12));
//#             keypad.put(new Integer(App.Const.KEY_M), new Integer(13));
//#             keypad.put(new Integer(App.Const.KEY_SPACE), new Integer(14));
        //#elifdef _8320
//#             keypad.put(new Integer(App.Const.KEY_Q), new Integer(0));
//#             keypad.put(new Integer(App.Const.KEY_W), new Integer(1));
//#             keypad.put(new Integer(App.Const.KEY_E), new Integer(2));
//#             keypad.put(new Integer(App.Const.KEY_R), new Integer(3));
//#             keypad.put(new Integer(App.Const.KEY_T), new Integer(4));
//#             keypad.put(new Integer(App.Const.KEY_Y), new Integer(5));
//#             keypad.put(new Integer(App.Const.KEY_U), new Integer(6));
//#             keypad.put(new Integer(App.Const.KEY_I), new Integer(7));
//#             keypad.put(new Integer(App.Const.KEY_O), new Integer(8));
//#             keypad.put(new Integer(App.Const.KEY_P), new Integer(9));
//#             keypad.put(new Integer(App.Const.KEY_A), new Integer(10));
//#             keypad.put(new Integer(App.Const.KEY_S), new Integer(11));
//#             keypad.put(new Integer(App.Const.KEY_D), new Integer(12));
//#             keypad.put(new Integer(App.Const.KEY_F), new Integer(13));
//#             keypad.put(new Integer(App.Const.KEY_G), new Integer(14));
//#             keypad.put(new Integer(App.Const.KEY_H), new Integer(15));
//#             keypad.put(new Integer(App.Const.KEY_J), new Integer(16));
//#             keypad.put(new Integer(App.Const.KEY_K), new Integer(17));
//#             keypad.put(new Integer(App.Const.KEY_L), new Integer(18));
//#             keypad.put(new Integer(App.Const.KEY_Z), new Integer(19));
//#             keypad.put(new Integer(App.Const.KEY_X), new Integer(20));
//#             keypad.put(new Integer(App.Const.KEY_C), new Integer(21));
//#             keypad.put(new Integer(App.Const.KEY_V), new Integer(22));
//#             keypad.put(new Integer(App.Const.KEY_B), new Integer(23));
//#             keypad.put(new Integer(App.Const.KEY_N), new Integer(24));
//#             keypad.put(new Integer(App.Const.KEY_M), new Integer(25));
//#             keypad.put(new Integer(App.Const.KEY_SPACE), new Integer(26));
        //#endif
//#         }
//#endif
        useAutoscroll = true;
        anchor = Graphics.TOP | Graphics.LEFT;
        stringLengths = new Vector();
        this.font = font;
        this.multiline = multiline;
        this.editable = editable;

        dx = App.Core.SCREEN_WIDTH;

        text = new char[App.Const.GUITEXT_MAXLEN];
        textLen = 0;
        tempCharIndex = -1;
        tempButton = -1;
        tempChar = App.Const.CHAR_EMPTY;
        nonpressedFrames = App.Const.GUITEXT_NONPRESSEDFRAMES;
        isCapital = false;
        cursorPos = 0;
        isPassword = false;
        drawOffset = 0;
    }

    public Object copy(Object from)
    {
        GUIText copy = (GUIText) from;
        copy.textLen = textLen;
        System.arraycopy(text, 0, copy.text, 0, textLen);
        copy.isPassword = isPassword;
        copy.useAutoscroll = useAutoscroll;
        copy.anchor = anchor;
        int size = stringLengths.size();

        copy.stringLengths.ensureCapacity(size);
        for (int i = 0; i < size; ++i)
        {
            copy.stringLengths.addElement(stringLengths.elementAt(i));
        }

        return super.copy(copy);
    }

    public Object clone()
    {
        GUIText newObj = new GUIText(font, multiline, editable);
        return copy(newObj);
    }

    public void applyTempSymbol()
    {
        char ch = chars[tempButton][tempCharIndex];
        ch = isCapital ? Character.toUpperCase(ch) : ch;
        System.arraycopy(text, cursorPos, text, cursorPos + 1, textLen - cursorPos);
        text[cursorPos] = ch;
        textLen++;
        cursorPos++;
        parseText();
        tempCharIndex = -1;
        tempButton = -1;
        tempChar = App.Const.CHAR_EMPTY;
    }

    public void update()
    {
        if (useAutoscroll && (!multiline) && (!editable) && (drawLength > dx))
        {
            drawOffset += 3;
            if (drawOffset > dx)
            {
                drawOffset = -dx;
            }
        }

        if (isPassword && textPassword == null)
        {
            textPassword = new char[App.Const.GUITEXT_MAXPASSLEN];
            for (int i = 0; i < App.Const.GUITEXT_MAXPASSLEN; ++i)
            {
                textPassword[i] = '*';
            }
        }

        if (editable && isSelected)
        {
            if (cursorFlashFrames++ > App.Const.GUITEXT_CURSOR_FLASH_FRAMES)
            {
                cursorFlashFrames = 0;
            }

            if (nonpressedFrames != 0)
            {
                nonpressedFrames--;
            }

            if ((-1 != tempCharIndex) && (0 == nonpressedFrames))
            {
                applyTempSymbol();
            }

            switch (App.Core.keyAction)
            {
                case App.Const.KEY_NEGATIVE:
                    if (textLen > 0)
                    {
                        System.arraycopy(text, cursorPos, text, cursorPos - 1, textLen - cursorPos);
                        cursorPos--;
                        textLen--;
                        parseText();
                    }
                    break;
                case App.Const.KEY_LEFT:
                    if (cursorPos > 0)
                    {
                        cursorPos--;
                    }
                    break;
                case App.Const.KEY_RIGHT:
                    if (cursorPos < textLen)
                    {
                        cursorPos++;
                    }
                    break;
                case App.Const.KEY_POUND:
                    isCapital = !isCapital;
                    break;
//#ifdef BLACKBERRY
        //#ifdef _8120
//#                 case App.Const.KEY_Q:
//#                 case App.Const.KEY_E:
//#                 case App.Const.KEY_T:
//#                 case App.Const.KEY_U:
//#                 case App.Const.KEY_O:
//#                 case App.Const.KEY_A:
//#                 case App.Const.KEY_D:
//#                 case App.Const.KEY_G:
//#                 case App.Const.KEY_J:
//#                 case App.Const.KEY_L:
//#                 case App.Const.KEY_Z:
//#                 case App.Const.KEY_C:
//#                 case App.Const.KEY_B:
//#                 case App.Const.KEY_M:
//#                 case App.Const.KEY_SPACE:
        //#elifdef _8320
//#                 case App.Const.KEY_Q:
//#                 case App.Const.KEY_W:
//#                 case App.Const.KEY_E:
//#                 case App.Const.KEY_R:
//#                 case App.Const.KEY_T:
//#                 case App.Const.KEY_Y:
//#                 case App.Const.KEY_U:
//#                 case App.Const.KEY_I:
//#                 case App.Const.KEY_O:
//#                 case App.Const.KEY_P:
//#                 case App.Const.KEY_A:
//#                 case App.Const.KEY_S:
//#                 case App.Const.KEY_D:
//#                 case App.Const.KEY_F:
//#                 case App.Const.KEY_G:
//#                 case App.Const.KEY_H:
//#                 case App.Const.KEY_J:
//#                 case App.Const.KEY_K:
//#                 case App.Const.KEY_L:
//#                 case App.Const.KEY_Z:
//#                 case App.Const.KEY_X:
//#                 case App.Const.KEY_C:
//#                 case App.Const.KEY_V:
//#                 case App.Const.KEY_B:
//#                 case App.Const.KEY_N:
//#                 case App.Const.KEY_M:
//#                 case App.Const.KEY_SPACE:
        //#endif  
//#                     {
//#                         int newButton = ((Integer) (keypad.get(new Integer(App.Core.keyAction)))).intValue();
//#                         if (App.Core.isAlt)
//#                         {
//#                             App.Core.isAlt = false;
//#                             tempButton = newButton;
//#                             tempCharIndex = 0;
//#                             applyTempSymbol();
//#                             break;
//#                         }
//# 
//#                         if ((tempButton != newButton && tempButton != -1) || (tempCharIndex == (chars[newButton].length - 1)))
//#                         {
//#                             applyTempSymbol();
//#                             tempCharIndex = 1;
//#                         } else
//#                         {
//#                             tempCharIndex = tempCharIndex == -1 ? 1 : tempCharIndex + 1;
//#                         }
//#                         tempButton = newButton;
//# 
//#                         nonpressedFrames = App.Const.GUITEXT_NONPRESSEDFRAMES;
//#                         tempChar = chars[tempButton][tempCharIndex];
//#                     }
//#                     break;
//#else
                case App.Const.KEY_0:
                case App.Const.KEY_1:
                case App.Const.KEY_2:
                case App.Const.KEY_3:
                case App.Const.KEY_4:
                case App.Const.KEY_5:
                case App.Const.KEY_6:
                case App.Const.KEY_7:
                case App.Const.KEY_8:
                case App.Const.KEY_9:
                    {
                        int newButton = App.Core.keyAction - App.Const.KEY_0;
                        if (tempButton != newButton && tempButton != -1)
                        {
                            applyTempSymbol();
                            tempCharIndex = 0;
                        } else
                        {
                            tempCharIndex++;
                        }
                        tempButton = newButton;

                        nonpressedFrames = App.Const.GUITEXT_NONPRESSEDFRAMES;
                        if (tempCharIndex > (chars[tempButton].length - 1))
                        {
                            tempCharIndex = 0;
                        }
                        tempChar = chars[tempButton][tempCharIndex];
                    }
                    break;
//#endif
            }
        }
    }

    public void render(int offsetX, int offsetY)
    {
        if (text == null)
        {
            return;
        }

        super.render(offsetX, offsetY);

        App.Core.staticG.setFont(font);
        //int oldColor = Core.staticG.getColor();
        //Core.staticG.setColor(0);

        if ((anchor & Graphics.HCENTER) == 1)
        {
            offsetX += dx / 2;
        }

        if ((anchor & Graphics.RIGHT) == 1)
        {
            offsetX += dx;
        }

        if ((!multiline) && (!editable) && (drawLength > dx))
        {
            offsetX -= drawOffset;
        }


        int tempLen = textLen;
        char charCursor = ' ';
        if (editable && isSelected)
        {
            charCursor = cursorFlashFrames > (App.Const.GUITEXT_CURSOR_FLASH_FRAMES / 2) ? ' ' : App.Const.CHAR_CURSOR;
            text[tempLen++] = charCursor;
            if (tempChar != App.Const.CHAR_EMPTY)
            {
                tempChar = isCapital ? Character.toUpperCase(tempChar) : tempChar;
                text[tempLen++] = tempChar;
            }
        }

        if (multiline)
        {
            int count = stringLengths.size();
            if (count == 0)
            {
                App.Core.staticG.drawChar(charCursor, x + offsetX, y + offsetY, anchor);
                if (tempChar != App.Const.CHAR_EMPTY)
                {
                    App.Core.staticG.drawChar(tempChar, x + offsetX + font.charWidth(tempChar), y + offsetY, anchor);
                }
            }
            int offset = 0;
            int len;
            for (int i = 0; i < count; ++i)
            {
                len = ((Integer) stringLengths.elementAt(i)).intValue();
                if (editable && (offset <= cursorPos) && (offset + len >= cursorPos))
                {
                    int firstLen = font.charsWidth(text, offset, cursorPos - offset);
                    int secondLen = firstLen + font.charWidth(charCursor);
                    int thirdLen = secondLen + (tempChar == App.Const.CHAR_EMPTY ? 0 : font.charWidth(tempChar));
                    App.Core.staticG.drawChars(text, offset, cursorPos - offset, x + offsetX, y + offsetY, anchor);
                    App.Core.staticG.drawChar(charCursor, x + offsetX + firstLen, y + offsetY, anchor);
                    if (tempChar != App.Const.CHAR_EMPTY)
                    {
                        App.Core.staticG.drawChar(tempChar, x + offsetX + secondLen, y + offsetY, anchor);
                    }
                    App.Core.staticG.drawChars(text, cursorPos, len + offset - cursorPos, x + offsetX + thirdLen, y + offsetY, anchor);
                } else
                {
                    App.Core.staticG.drawChars(text, offset, len, x + offsetX, y + offsetY, anchor);
                }
                offsetY += font.getHeight();
                offset += len + 1;
            }
        } else
        {
            char[] textToDraw = isPassword ? textPassword : text;
            int len = font.charsWidth(textToDraw, 0, tempLen);
            int offset = len > dx ? len - dx : 0;
            int cursotOffset = font.charsWidth(textToDraw, 0, cursorPos);
            if (cursotOffset < offset)
            {
                offset = cursotOffset;
            }
            int finalY = y + offsetY + (dy - font.getHeight()) / 2;
            if (editable && isSelected)
            {
                int firstLen = font.charsWidth(textToDraw, 0, cursorPos);
                int secondLen = firstLen + font.charWidth(charCursor);
                int thirdLen = secondLen + (tempChar == App.Const.CHAR_EMPTY ? 0 : font.charWidth(tempChar));
                App.Core.staticG.drawChars(textToDraw, 0, cursorPos, x + offsetX - offset, finalY, anchor);
                App.Core.staticG.drawChar(charCursor, x + offsetX - offset + firstLen, finalY, anchor);
                if (tempChar != App.Const.CHAR_EMPTY)
                {
                    App.Core.staticG.drawChar(tempChar, x + offsetX - offset + secondLen, finalY, anchor);
                }
                App.Core.staticG.drawChars(textToDraw, cursorPos, textLen - cursorPos, x + offsetX - offset + thirdLen, finalY, anchor);
            } else
            {
                App.Core.staticG.drawChars(textToDraw, 0, textLen, x + offsetX - offset, finalY, anchor);
            }
        }
    //Core.staticG.setColor(oldColor);
    }

    public void setText(String text)
    {
        textLen = text.length();
        System.arraycopy(text.toCharArray(), 0, this.text, 0, textLen);
        parseText();
        cursorPos = textLen;
        drawLength = font.charsWidth(this.text, 0, textLen);
    }

    private void parseText()
    {
        if (multiline)
        {
            String tempStr = new String(text, 0, textLen);
            text[textLen + 1] = ' ';
            stringLengths.removeAllElements();
            int pos = 0;
            int lastPos = 0;
            int lastIndex = 0;
            boolean isComplete = false;

            while (true)
            {
                pos = tempStr.indexOf(' ', lastPos);
                if (-1 == pos)
                {
                    pos = tempStr.length();
                    isComplete = true;
                }
                if (font.substringWidth(tempStr, lastIndex, pos - lastIndex) > dx)
                {
                    stringLengths.addElement(new Integer(lastPos - lastIndex - 1));
                    lastIndex = lastPos;
                }
                lastPos = pos + 1;

                if (isComplete)
                {
                    if (lastIndex != pos)
                    {
                        stringLengths.addElement(new Integer(pos - lastIndex));
                    }
                    break;
                }
            }
        }
    }
}
