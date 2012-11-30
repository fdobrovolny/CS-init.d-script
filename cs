#!/bin/bash
# /etc/init.d/cs

# chconfig: 2345 70 70

### BEGIN INIT INFO
# Provides:   cs
# Required-Start: $local_fs $remote_fs $mysqld
# Required-Stop:  $local_fs $remote_fs $mysqld
# Should-Start:   $network
# Should-Stop:    $network
# Default-Start:  2 3 4 5
# Default-Stop:   0 1 6
# Short-Description:    Counter-strike server
# Description:    Init script for Counter-strike server.
### END INIT INFO

# Created by Floriusin

# Pre-Based on https://github.com/Ahtenus/minecraft-init
# Based on https://github.com/BrnoPCmaniak/WOW-init.d-script
# Updates on https://github.com/BrnoPCmaniak/CS-init.d-script

# Loads config file
PATH=$PWD
CD $PATH
if [ ! -d servers]
then
    echo "Creating dir servers"
    mkdir servers
fi

#if [ -L $0 ]
#then
#        source `readlink -e $0 | sed "s:[^/]*$:config:"`
#else
#        source `echo $0 | sed "s:[^/]*$:config:"`
#fi
#
#if [ "$SALT" == "" ]
#then
#        echo "Couldn't load config file, please edit config.example and rename it to config"
#        logger -t cs-init-config "Couldn't load config file, please edit config.example and rename it to config"
#        exit
#fi

ME=`whoami`
as_user() {
        if [ $ME == $USERNAME ] ; then
                bash -c "$1"
        else
                su $USERNAME -s /bin/bash -c "$1"
        fi
}
is_running($port){
        # Checks for the world server screen session
        # returns true if it exists.
        lsof -i:$port > /dev/null && return 0
        lsof -i:$port > /dev/null || return 1
}
start($port) {
  cd $PATH/servers
  source `readlink -e $0 | sed "s:[^/]*$:$id:"`
	cd $SERVPATH
	as_user "cd $SERVERPATH && screen -dmS $SCREEN $INVOCATION"
	# Waiting for the server to start
	seconds=5
	until ps ax | grep -v grep | grep "$SCREEN $INVOCATION" > /dev/null
	do
		sleep 1
		seconds=$seconds+1
		if [[ $seconds -eq 5 ]]
		then
			echo "Still not running, waiting a while longer..."
		fi
		if [[ $seconds -ge 120 ]]
		then
			echo -e "Server \"$NAME\" on port $port failed to start, aborting.    [ \033[0;31mFAIL\033[0m ]"
			exit 1
		fi
	done	
	echo -e "Server \"$NAME\" on port $port is running.                 [  \033[0;32mOK\033[0m  ]"
}
stop($port) {
  cd $PATH/servers
  source `readlink -e $0 | sed "s:[^/]*$:$port:"`
	as_user "screen -p 0 -S $SCREEN -X eval 'stuff \"^C\"\015'"
	sleep 0.5
	# Waiting for the server to shut down
	seconds=0
	while ps ax | grep -v grep | grep "$SCREEN $INVOCATION" > /dev/null
	do
		sleep 1 
		seconds=$seconds+1
		if [[ $seconds -eq 5 ]]
		then
			echo "Still not shut down, waiting a while longer..."
		fi
		if [[ $seconds -ge 120 ]]
		then
			logger -t cs-init "Failed to shut down server \"$NAME\" on port $port, aborting."
			echo -e "Server \"$NAME\" on port $port failed to start, aborting.     [ \033[0;31mFAIL\033[0m ]"
			exit 1
		fi
	done	
	echo -e "Server \"$NAME\" on port $port is now shut down.   [  \033[0;32mOK\033[0m  ]"
}
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
case "$1" in
  start)
    # Starts the server
    cd $PATH/servers
    soubory=`ls |grep $2$`
    for i in $soubory; do
    start($i)
    done
		;;
	stop)
		# Stops the server
    cd $PATH/servers
    soubory=`ls |grep $2$`
    for i in $soubory; do
    stop($i)
    done
		;;
	restart)
    # Restart servers
    cd $PATH/servers
    soubory=`ls |grep $2$`
    for i in $soubory; do
    stop($i)
    sleep 5
    start($i)
    done
		;;
	status)
		# Shows servers status
    cd $PATH/servers
    soubory=`ls |grep $2$`
    for i in $soubory; do
    is_running($i)
    done
		;;
  menu)
		# Shows menu
    echo -en "\033[s"
    while [ $bolean ] do
    echo -en "\033[u"
    main_menu()
    read option
    if [ "$option" == "1" ]; than
      list_servers()
    elif [ "$option" == "2" ]; than
      new_server()
    elif [ "$option" == "3" ]; than
      rm_server()
    elif [ "$option" == "4" ]; than
      echo "Good Bay.."
      sleep 5
      echo -en "\033[u"
      echo -en "\033[14A"
      tput ed
      bolean=false
    else
      echo "Option $option doesn't exist!"
      sleep 5
    done
    
		;;
	help|--help|-h)
		echo "Usage: $0 COMMAND"
		echo 
		echo "Available commands:"
		echo -e "   start \t\t Starts the server"
		echo -e "   stop \t\t Stops the server"
		echo -e "   restart \t\t Restarts the server"
		echo -e "   status \t\t Displays server status"
		;;
	*)
		echo "Usage: wow {start|stop|restart|status|menu|help}"
		exit 1
		;;

exit 0