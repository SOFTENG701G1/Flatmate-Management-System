import React, {Component} from 'react';
import '../App.css';
import Container from 'react-bootstrap/Container';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import './chores.css';
import ChoresTable from './ChoresTable';

let title = "All" /* Placeholder tab names/content until we can retrieve from backend and chores table implemented */

export default class Chores extends Component {

    constructor(props) {
        super(props)
        this.state = {
            chores: [
                {
                    chore: "Do dishes",
                    assignee: "Yin",
                    completeBy: "17th March",
                    completed: false
                },
                {
                    chore: "Vacuum",
                    assignee: "Yin",
                    completeBy: "18th March",
                    completed: false
                },
                {
                    chore: "Tidy lounge",
                    assignee: "Teresa",
                    completeBy: "19th March",
                    completed: true
                },
                {
                    chore: "Do the washing",
                    assignee: "Bryan",
                    completeBy: "19th March",
                    completed: true
                },
            ]
        }
    }

    render () {
        return (
            <Container>
                <h2>Flat Chores</h2>
                <Tabs>
                    <TabList>
                        <Tab>{title}</Tab>    
                        <Tab>Yin</Tab>
                        <Tab>Teresa</Tab>
                        <Tab>Bryan</Tab>
                    </TabList>
                    
                    <TabPanel> 
                        <ChoresTable assignee="All" chores={this.state.chores}/>
                    </TabPanel>
                    <TabPanel>
                        <ChoresTable assignee="Yin" chores={this.state.chores.filter(chore => {
                            if ('Yin' == chore.assignee ) {
                                return chore;
                            }})}/>
                    </TabPanel>
                    <TabPanel>
                        <ChoresTable assignee="Teresa" chores={this.state.chores.filter(chore => {
                            if ('Teresa' == chore.assignee ) {
                                return chore;
                            }})}/>
                    </TabPanel>
                    <TabPanel>
                        <ChoresTable assignee="Bryan" chores={this.state.chores.filter(chore => {
                            if ('Bryan' == chore.assignee ) {
                                return chore;
                            }})}/>
                    </TabPanel>
                </Tabs>
                Button to add new chores
            </Container>
            );
    }
}