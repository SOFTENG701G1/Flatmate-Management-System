import React, {Component} from 'react';
import '../App.css';

import Container from 'react-bootstrap/Container';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import './chores.css';

let title = "All" /* Placeholder tab names/content until we can retrieve from backend and chores table implemented */

export default class Chores extends Component {
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
                    
                    <TabPanel> {/* Large Lorem text included as a test for the scroll bar */}
                        <h2>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam nec libero sit amet quam porta condimentum. 
                            Sed elementum mauris ipsum, quis accumsan risus aliquet ullamcorper. Ut lobortis ac velit auctor iaculis. 
                            Nunc pulvinar sodales nunc sed viverra. Sed egestas, tellus eget tempor gravida, orci mi eleifend lacus,
                            ut sagittis felis arcu eu nulla. Vestibulum pretium, est a finibus blandit, odio 
                            nibh dignissim nisl, sed mattis orci lectus id augue. Praesent a porta nisi. Nullam luctus quis sapien in vehicula.
                            Fusce in tristique ipsum. Vestibulum et fermentum ipsum. Proin blandit ut mi venenatis consectetur.
                        </h2>
                    </TabPanel>
                    <TabPanel>
                        <h2>List of Yin's Chores</h2>
                    </TabPanel>
                    <TabPanel>
                        <h2>List of Teresa's Chores</h2>
                    </TabPanel>
                    <TabPanel>
                        <h2>List of Bryan's Chores</h2>
                    </TabPanel>
                </Tabs>
                Button to add new chores
            </Container>
            );
    }
}