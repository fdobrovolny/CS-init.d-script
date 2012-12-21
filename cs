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
##################################################################
# Created by Floriusin                                           #
##################################################################
# Pre-Based on https://github.com/Ahtenus/minecraft-init         #
# Based on https://github.com/BrnoPCmaniak/WOW-init.d-script     #
# Updates on https://github.com/BrnoPCmaniak/CS-init.d-script    #
##################################################################
# Config                                                         #
  USERNAME="root"                                                #
##################################################################

# Loads config file
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
PATH=$PWD
as_user "cd $PATH"
if [ ! -d servers ]
then
    echo "Creating dir servers"
    as_user "mkdir servers"
fi
is_running($1) {
        # Checks for the world server screen session
        # returns true if it exists.
        lsof -i:$port > /dev/null && return 0
        lsof -i:$port > /dev/null || return 1
}
start($1) {
  port=$1
  cd $PATH/servers
  source `readlink -e $0 | sed "s:[^/]*$:$port:"`
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
stop($1) {
  port=$1
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
list($1) {
  port=$1
  cd $PATH/servers
  source `readlink -e $0 | sed "s:[^/]*$:$port:"`
  echo -e "On port $port is server with name \"$NAME\" \n"
}
create() {
  cd $PATH/servers
  echo -e "Enter name of server"
  read $name
  echo -e "Enter port of server with name $NAME"
  read $port
  echo -e "Enter path of directory with server(main folder) use something like \"\\opt\\cs\\016\" not \"\\opt\\cs\\016\\cstrike\""
  read $serverpath
  touch $port
  echo -e "###############################################\n"
  echo -e "# Config file of server on port $port         #\n"
  echo -e "###############################################\n"
  echo -e "#port\n"
  echo -e "PORT=$port \n"
  echo -e "#name\n"
  echo -e "NAME=$name \n"
  echo -e "#server path\n"
  echo -e "SERVERPATH=$serverpath \n"
  source `readlink -e $0 | sed "s:[^/]*$:$port:"`
  if [ $PORT == $port && $NAME == $name && $SERVERPATH == $serverpath ] ; then
    echo "Server suceful created"
  else
    echo "Sorry somewhere is error"
  fi
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
  list)
    # Lists servers
    cd $PATH/servers
    soubory=`ls |grep $2$`
    for i in $soubory; do
      list($i)
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