package main

import (
	"fmt"
	"log"
	"net"
	"time"
)

//region Server Properties

const (
	port             string = "52515"
	totalPlayerLimit int    = 16
	dataBufferSize   int    = 4096
)

var serverIsActive bool

//ServerIsActive returns true if and only if the server is currently active and listening.
func ServerIsActive() bool {
	return serverIsActive
}

//endregion

//region Server Initialization and Listening

func initializeServerParams() {
	initializeClientManagementParams()
}

func main() {

	initializeServerParams()

	listener, err := net.Listen("tcp", port)

	if err != nil {
		fmt.Println(err)
		return
	}

	defer listener.Close()

	fmt.Println("Now listening on port " + port + "...")

	serverIsActive = true

	go delegateChannels()

	for serverIsActive {
		conn, err := listener.Accept()
		if err != nil {
			log.Fatalln(err.Error()) //in which cases is an error raised? is terminating entire server worth this?
		}

		fmt.Println("New connection from " + conn.RemoteAddr().String())
		go processInitialConnection(&conn)
	}
}

//endregion

//region Connection and Client/Player Construction

func processInitialConnection(conn *net.Conn) {
	p := constructBasePlayerIfValid(conn)
	if p == nil {
		fmt.Println(time.Now().Format("2006-01-02 15:04:05") + ": " + "INVALID CONNECTION FROM: " + (*conn).RemoteAddr().String())
		(*conn).Close()
		return
	}

}

func constructBasePlayerIfValid(conn *net.Conn) *player {
	id := -1

	//try to find valid id TODO: Optimize
	for i := 0; i < totalPlayerLimit; i++ {
		if idToPlayer[i] != nil {
			id = i
		}
	}

	if id == -1 {
		//no valid id could be found! Server is over limit.
		return nil
	}

	return newPlayer(id, conn)
}

//endregion

//region Client Processing

func tendToClientRead(p *player) {

}

//endregion

//region Delegate Channels and Server Control

func closeServer() {
	//handle all things that need to be shut down. send appropriate messages to all clients etc.
	serverIsActive = false
}

var serverCloseChannel = make(chan interface{})

func delegateChannels() {
	for serverIsActive {
		select {
		case <-serverCloseChannel:
			closeServer()
			break
		}
	}
}

//endregion
