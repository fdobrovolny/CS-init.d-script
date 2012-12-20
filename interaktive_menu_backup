#cs interaktive
#
# comad form
  menu)
		# Shows menu
    echo -en "\033[s"
    while [ $bolean ] do
      echo -en "\033[u"
      main_menu()
      read option
      if [ "$option" == "1" ]; than
       echo -en "\033[u"
       list_servers()
      elif [ "$option" == "2" ]; than
       new_server()
      elif [ "$option" == "3" ]; than
        rm_server()
      elif [ "$option" == "4" ]; than
        echo "Good Bay..."
        sleep 5
        echo -en "\033[u"
        echo -en "\033[14A"
        tput ed
        bolean=flse
      else
        echo "Option $option doesn't exist!"
        sleep 5
      fi
    done
    
		;;
# comands
logo() {
  echo -e "\n
        CS init.d script v 1.0 made by Floriusin"
  echo "   ___                       _                       ___   _           _   _"
  echo "  / __|  ___   _  _   _ _   | |_   ___   _ _   ___  / __| | |_   _ _  (_) | |__  ___"
  echo " | (__  / _ \ | || | | ' \  |  _| / -_) | '_| |___| \__ \ |  _| | '_| | | | / / / -_)"
  echo "  \___| \___/  \_,_| |_||_|  \__| \___| |_|         |___/  \__| |_|   |_| |_\_\ \___|"
  echo "  ___          _   _            _                     _          _"
  echo " |_ _|  _ _   (_) | |_       __| |    ___  __   _ _  (_)  _ __  | |_"
  echo "  | |  | ' \  | | |  _|  _  / _\` |   (_-< / _| | '_| | | | '_ \ |  _|"
  echo " |___| |_||_| |_|  \__| (_) \__,_|   /__/ \__| |_|   |_| | .__/  \__|"
  echo "                                                         |_|"
  echo "  ___   ___   ___   ___   ___   ___   ___   ___   ___   ___   ___   ___   ___   ___   ___"
  echo " |___| |___| |___| |___| |___| |___| |___| |___| |___| |___| |___| |___| |___| |___| |___|"
  echo -e "\n"
}
main_menu() {
  echo "              1) List of Servers"
  echo "              2) New Server"
  echo "              3) Remove Server"
  echo "              4) Exit"
  echo "Enter your option: "
}
list_servers() {
  cd $PATH/servers
  while [ $bolean ] do
      echo -en "\033[u"
      echo "              0) Exit"
      soubory=`ls |grep $2$`
      for i in $soubory; do
        source `readlink -e $0 | sed "s:[^/]*$:$id:"`
        echo "              $i) $NAME"
      done
      echo "Enter your option: "
      read option
      if [ "$option" == "0" ]; than
        echo -en "\033[u"
        tput ed
        bolean=flse
      elif [ -f $option ]; than
        server_edit($option)
      else
        echo "Option $option doesn't exist!"
        sleep 5
      fi  
  done
}