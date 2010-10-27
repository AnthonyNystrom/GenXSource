cd sprite
bmp2spa.exe header.bmp 1 -t
bmp2spa.exe footer.bmp 3 -t
bmp2spa.exe item.bmp 2 -cFF00FF
bmp2spa.exe itembig.bmp 2 -cFF00FF
bmp2spa.exe icons.bmp 6
bmp2spa.exe dashicons.bmp 4
bmp2spa.exe checkbox.bmp 2 -cFF00FF
bmp2spa.exe combobox.bmp 10 -cFF00FF
bmp2spa.exe shift.bmp 1 -cFF00FF
bmp2spa.exe net.bmp 1

copy *.spa ..\compiled\
copy *.png ..\compiled\
pause