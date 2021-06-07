#!/usr/bin/env python3
# -*- coding: utf-8 -*-

__author__ = "Mohd Hafizuddin Bin Kamilin"
__date__ = "28 May 2021"

# for random placement and branching
from random import randint, choice
# add waiting time before updating the minefield
from time import sleep

# class for generating a safe path to traverse via randomized single branch (path) depth first search (dfs)
class SafePathGenerator:

    def __init__(self, n, m):

        # get the traversal area where n (length) and m (height) is the max bounding area
        self.n = n
        self.m = m
        # initialize the 2d array (python list) for storing the totoshka's route, ally, safe path and
        # mine position on the minefield map
        self.minefieldMap = [[None] * self.n for _ in range(self.m)]
        
    # method to create a path from south to north 
    # (assuming the starting location for totoshka and ally is at the south side of the map)
    def createNow(self):

        # for storing the traversed nodes
        traversedNodes = []

        # consider that counting the rows and columns start from zero
        column = self.n - 1
        row = self.m - 1

        # randomize the position of the root node
        # currentNode[0] = column, currentNode[1] = row
        currentNode = [randint(0, column), row]
        traversedNodes.append(currentNode)

        # because we only want to create a tree with a single branch (in other word, a path),
        # we don't use recursive for the dfs
        while True:

            # keep on randomizing the direction until a traversable path is found
            while True:

                # NOTE: because we want to traverse from south to north, we ommited the movement from
                #       south-east, south and south-west 

                #   --  x-  +-      ↖  ↑  ↗
                #   -y  xy  +y      ←     → 
                #   -+  x+  ++      ↙  ↓  ↘     } movements in this section is ommited

                # randomly pick the branch direction with bias
                # north          0 <= α < 30      30%
                # east          30 <= α < 45      15%
                # north-east    45 <= α < 65      20%
                # west          65 <= α < 80      15%
                # north-west    80 <= α < 100     20%
                randomizedDirection = randint(0, 99)

                # check if it can traverse to north
                if ((0 <= currentNode[1] - 1 <= row) and (0 <= randomizedDirection < 30)):

                    temp = [currentNode[0], currentNode[1] - 1]

                    # if the newly generated position for the node to traverse have not yet been traversed
                    if not (temp in traversedNodes):

                        # traverse to this node
                        currentNode = temp
                        traversedNodes.append(temp)
                        break

                # check if it can traverse to east
                elif ((0 <= currentNode[0] + 1 <= column) and (30 <= randomizedDirection < 45)):

                    temp = [currentNode[0] + 1, currentNode[1]]

                    # if the newly generated position for the node to traverse have not yet been traversed
                    if not (temp in traversedNodes):

                        # traverse to this node
                        currentNode = temp
                        traversedNodes.append(temp)
                        break

                # check if it can traverse to north-east
                elif ((0 <= currentNode[0] + 1 <= column) and (0 <= currentNode[1] - 1 <= row) and (45 <= randomizedDirection < 65)):

                    temp = [currentNode[0] + 1, currentNode[1] - 1]

                    # if the newly generated position for the node to traverse have not yet been traversed
                    if not (temp in traversedNodes):

                        # traverse to this node
                        currentNode = temp
                        traversedNodes.append(temp)
                        break

                # check if it can traverse to west
                elif ((0 <= currentNode[0] - 1 <= row) and (65 <= randomizedDirection < 80)):

                    temp = [currentNode[0] - 1, currentNode[1]]

                    # if the newly generated position for the node to traverse have not yet been traversed
                    if not (temp in traversedNodes):

                        # traverse to this node
                        currentNode = temp
                        traversedNodes.append(temp)
                        break

                # check if it can traverse to north-west
                elif ((0 <= currentNode[0] - 1 <= column) and (0 <= currentNode[1] - 1 <= row) and (80 <= randomizedDirection < 100)):

                    temp = [currentNode[0] - 1, currentNode[1] - 1]

                    # if the newly generated position for the node to traverse have not yet been traversed
                    if not (temp in traversedNodes):

                        # traverse to this node
                        currentNode = temp
                        traversedNodes.append(temp)
                        break

            # if the traversed node is the final node
            if (currentNode[1] == 0):

                break

        # for each individual list (representing the xy-axis of the safe path)...
        for node in traversedNodes:
            
            # retranslate it into an empty position on self.minefieldMap
            self.minefieldMap[node[1]][node[0]] = 0

        # for each row
        for i in range(self.m):

            # for each column
            for j in range(self.n):

                # if the array location is empty
                if (self.minefieldMap[i][j] is None):

                    # choose either to place an empty field (0) or a bomb (2)
                    self.minefieldMap[i][j] = choice([0, 2])
            
            # retranslate it into a safe position on self.minefieldMap
            self.minefieldMap[node[1]][node[0]] = 0

        return self.minefieldMap

# class for displaying the top view of the minefield
class DisplayMinefield:

    # empty field = 0
    # √ = totoshka's route = 1
    # X = bomb = 2
    # (^_^) = ally = 3
    # \(°ᴥ°)ﾉ = totoshka = 4   
    # 
    # topview of the minefield
    # +---------+----
    # |         |    
    # +---------+----
    # |         |

    def __init__(self, n, m, minefieldMap):

        # get the size of the minefield where n is length and m is height
        self.n = n
        self.m = m
        # 2d array (python list) for storing the totoshka's route, ally, safe path and
        # mine position on the minefield map
        self.minefieldMap = minefieldMap

    # method for translating totoshka's starting position into an ascii art
    def drawTotoshka(self, totoshkaStartingPosition):

        # ascii art string output
        displayContent = ""

        for position in range(self.n):

            # totoshka start at here
            if (position == totoshkaStartingPosition):

                displayContent += "  \\(°ᴥ°)ﾉ "

            # just an empty spacing
            else:

                displayContent += " " * 10

        print(displayContent)

    # method for translating ally's starting position into an ascii art
    def drawAlly(self, allyStartingPosition):

        # ascii art string output
        displayContent = ""

        for position in range(self.n):

            # ally start at here
            if (position == allyStartingPosition):

                displayContent += "   (^_^)  "

            # just an empty spacing
            else:

                displayContent += " " * 10

        print(displayContent)

    # method for translating the totoshka's route, ally, safe path and mine position into an ascii art
    def drawSymbols(self, input):

        # empty field = 0, totoshka's route = 1, bomb = 2, ally = 3, totoshka = 4
        return "         " if (input == 0) else "    √    " if (input == 1) else "    X    " if (input == 2) else "  (^_^)  " if (input == 3) else " \\(°ᴥ°)ﾉ "

    # method for drawing the DisplayMinefield map
    def drawNow(self):

        # draw the bounding box
        for i in range(self.m):

            displayContent0 = "+" + "---------+" * self.n
            print(displayContent0)
            displayContent1 = "|"

            # draw with the totoshka's route, ally, safe path and mine position
            for j in range(self.n):

                displayContent1 += self.drawSymbols(self.minefieldMap[i][j]) + "|"

            print(displayContent1)

        print(displayContent0)

# class that emulate the traversing behavior of totoshka and ally via recursive dfs
class TraversingMinefield:

    def __init__(self, n, m, minefieldMap):

        # position of totoshka and ally when standing outside of the minefield perimeter
        self.outsidePosition = 0
        # recording the number of steps taken to reach the destination
        self.stepTaken = 0
        # get the traversal area where n (length) and m (height) is the max bounding area
        self.n = n
        self.m = m
        # consider that counting the rows and columns start from zero
        self.column = self.n - 1
        self.row = self.m - 1
        # get the generated minefieldMap from the SafePathGenerator
        self.minefieldMap = minefieldMap
        # path traversed by totoshka
        self.totoshkaTraversedPath = []
        # recursiveDFS's markers
        self.destinationReached = False
        # call DisplayMinefield here to update the map
        self.currentTopView = DisplayMinefield(n, m, minefieldMap)

    # method to update the current location of totoshka and ally on the map
    def updateMinefieldMap(self):
        sleep(2)
        self.stepTaken += 1

        # for each row
        for i in range(self.m):

            # for each column
            for j in range(self.n):

                # if the array location is not empty or has a bomb
                if ((self.minefieldMap[i][j] != 0) and (self.minefieldMap[i][j] != 2)):

                    # reset the array back to 0
                    self.minefieldMap[i][j] = 0

        if (self.destinationReached is False):

            pathLength = len(self.totoshkaTraversedPath)

            if (pathLength >= 2):

                print("\nStep " + str(self.stepTaken) + ":")
                # get the current location of totoshka and ally
                totoshkaLocation = self.totoshkaTraversedPath[-1]
                allyLocation = self.totoshkaTraversedPath[-2]

                # for each element in list (except the last 2 elements)
                for node in self.totoshkaTraversedPath[:-2]:
                    
                    # mark the field passed by totoshka to reach the destination on the self.minefieldMap
                    self.minefieldMap[node[1]][node[0]] = 1

                # mark the empty field with the current location of totoshka and ally
                minefieldMap[allyLocation[1]][allyLocation[0]] = 3
                minefieldMap[totoshkaLocation[1]][totoshkaLocation[0]] = 4

                # draw the map
                self.currentTopView.minefieldMap = minefieldMap
                self.currentTopView.drawNow()

            elif (pathLength == 1):

                print("\nStep " + str(self.stepTaken) + ":")
                # get the current location of totoshka and ally
                totoshkaLocation = self.totoshkaTraversedPath[0]

                # for each element in list (except the last element)
                for node in self.totoshkaTraversedPath[:-1]:
                    
                    # mark the field passed by totoshka to reach the destination on the self.minefieldMap
                    self.minefieldMap[node[1]][node[0]] = 1

                # mark the empty field with the current location of totoshka
                minefieldMap[totoshkaLocation[1]][totoshkaLocation[0]] = 4

                # draw the map
                self.currentTopView.minefieldMap = minefieldMap
                self.currentTopView.drawNow()
                self.currentTopView.drawAlly(totoshkaLocation[0])
                
            else:

                print("\nStep " + str(self.stepTaken) + ":")
                self.currentTopView.minefieldMap = minefieldMap
                self.currentTopView.drawNow()
                self.currentTopView.drawTotoshka(self.outsidePosition)
                self.currentTopView.drawAlly(self.outsidePosition)

        else:

            print("\nStep " + str(self.stepTaken) + ":")
            # get the current location of totoshka
            totoshkaLocation = self.totoshkaTraversedPath[-1]

            # NOTE: displaying the last 2 steps

            # for each element in list (except the last element)
            for node in self.totoshkaTraversedPath[:-1]:
                
                # mark the field passed by totoshka to reach the destination on the self.minefieldMap
                self.minefieldMap[node[1]][node[0]] = 1

            # mark the empty field with the current location of ally
            minefieldMap[totoshkaLocation[1]][totoshkaLocation[0]] = 3

            # draw the map
            self.currentTopView.drawTotoshka(totoshkaLocation[0])
            self.currentTopView.minefieldMap = minefieldMap
            self.currentTopView.drawNow()

            # NOTE: displaying the last step
            sleep(2)
            print("\nStep " + str(self.stepTaken + 1) + ":")

            # for each element in list 
            for node in self.totoshkaTraversedPath:
                
                # mark the field passed by totoshka to reach the destination on the self.minefieldMap
                self.minefieldMap[node[1]][node[0]] = 1

            # draw the map
            self.currentTopView.drawTotoshka(totoshkaLocation[0])
            self.currentTopView.drawAlly(totoshkaLocation[0])
            self.currentTopView.minefieldMap = minefieldMap
            self.currentTopView.drawNow()


    # method to perform the recursive dfs method for finding the safe route
    def recursiveDFS(self, currentNode):

        # sniff the adjacent based on these priorities
        # 1: north, 2: north-east, 3: north-west, 4: east, 5: west 
        for direction in range(5):

            # check if it can traverse to north
            if ((direction == 0) and (0 <= currentNode[1] - 1 <= self.row)):

                temp = [currentNode[0], currentNode[1] - 1]

            # check if it can traverse to north-east
            elif ((direction == 1) and (0 <= currentNode[0] + 1 <= self.column) and (0 <= currentNode[1] - 1 <= self.row)):

                temp = [currentNode[0] + 1, currentNode[1] - 1]

            # check if it can traverse to north-west
            elif ((direction == 2) and (0 <= currentNode[0] - 1 <= self.column) and (0 <= currentNode[1] - 1 <= self.row)):

                temp = [currentNode[0] - 1, currentNode[1] - 1]

            # check if it can traverse to east
            elif ((direction == 3) and (0 <= currentNode[0] + 1 <= self.column)):

                temp = [currentNode[0] + 1, currentNode[1]]

            # check if it can traverse to west
            elif ((direction == 4) and (0 <= currentNode[0] - 1 <= self.row)):

                temp = [currentNode[0] - 1, currentNode[1]]

            # if the newly generated possible node to traverse have not yet been traversed and the node is an empty field
            if ((not (temp in self.totoshkaTraversedPath)) and (self.minefieldMap[temp[1]][temp[0]] == 0)):

                # append it to the self.totoshkaTraversedPath
                self.totoshkaTraversedPath.append(temp)
                # update the map
                self.updateMinefieldMap()

                # if the node is our final destination
                if (temp[1] == 0):

                    # stop the recursion
                    self.destinationReached = True
                    break

                else:

                    # perform recursive operation
                    self.recursiveDFS(temp)

                    # if destination is not reachable even after the recursion
                    if (self.destinationReached is False):

                        # remove the newly added node
                        self.totoshkaTraversedPath.pop()
                        self.updateMinefieldMap()

                    # else if final destination was reached
                    else:

                        break

    # method that emulate the behavior of totoshka and ally when traversing the minefield
    def moveNow(self):

        # for each entry point (root node) of the minefield, find the first entry point that
        # can reach the destination
        for i in range(self.n):

            # update totoshka and ally position outside of the minefield perimeter
            self.outsidePosition = i
            # update the minefieldMap, showing totoshka and ally standing outside before entering the minefield perimeter
            self.updateMinefieldMap()

            # find a node with an empty field to be picked as a root node
            if (self.minefieldMap[self.row][i] != 2):

                # the temp node is the current node totoshka's is standing right now
                temp = [i, self.row]
                # record totoshka's position
                self.totoshkaTraversedPath.append(temp)
                self.updateMinefieldMap()
                # call the recursiveDFS to search the safe path to reach the destination
                self.recursiveDFS(temp)

                # if destination reached
                if (self.destinationReached is True):

                    # update the minefieldMap, showing totoshka and ally standing outside before entering the minefield perimeter
                    self.updateMinefieldMap()
                    break

                # reach the dead-end route instead
                else:

                    # remove the newly added node
                    self.totoshkaTraversedPath.pop()
                    self.updateMinefieldMap()

# main
if __name__ == "__main__":

    print("\nThis program will create a minefield with randomly placed bombs and a safe path.")
    print("From there, it will simulate the traversing behavior of Ally and Totoshka.")

    # show the legends used for visualization representation
    print("\nLegends used:")
    print("  X = Bomb")
    print("  (^_^) = Ally")
    print("  \\(°ᴥ°)ﾉ = Totoshka")
    print("  √ = Totoshka's route")

    # get the input size to generate the minefield
    m = int(input("\nEnter the number of row for the minefield: "))
    n = int(input("Enter the number of column for the minefield: "))

    print("\nInitial condition.")
    # initialize a 2d array to represent a minefield that has an empty field and a bomb
    mineField = SafePathGenerator(n, m)
    minefieldMap = mineField.createNow()

    # draw the initial state of the minefield
    initialTopView = DisplayMinefield(n, m, minefieldMap)
    initialTopView.drawNow()
    # traverse the minefield
    pathTraversed = TraversingMinefield(n, m, minefieldMap)
    pathTraversed.moveNow()
    # shows the total steps taken to traverse the minefield
    print("\n" + str(pathTraversed.stepTaken + 1) + " steps taken to traverse the minefield.\n")
