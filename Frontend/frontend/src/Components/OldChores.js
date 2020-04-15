import React, { Component } from "react";
import "../App.css";
import Container from "react-bootstrap/Container";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import ChoresUpdateButton from "./ChoresUpdateButton";
import "./chores.css";
import ChoresTable from "./ChoresTable";

let title =
  "All"; /* Placeholder tab names/content until we can retrieve from backend and chores table implemented */
/**
 * This class creates the chores components which shows the user all of their chores and
 * all of the other people's chores on their flat.
 * They can also add or modiy chores and set chores to done
 */
export default class Chores extends Component {
  constructor(props) {
    super(props);
    this.state = {
      chores: [
        {
          chore: "Do dishes",
          assignee: "Yin",
          completeBy: "17th March",
          completed: false,
        },
        {
          chore: "Vacuum",
          assignee: "Yin",
          completeBy: "18th March",
          completed: false,
        },
        {
          chore: "Tidy lounge",
          assignee: "Teresa",
          completeBy: "19th March",
          completed: true,
        },
        {
          chore: "Do the washing",
          assignee: "Bryan",
          completeBy: "19th March",
          completed: true,
        },
      ],
    };
  }

  render() {
    return (
      <Container>
        <div
          class="section-header"
          style={{ width: "45%", marginBottom: "50px" }}
        >
          Flat Chores
        </div>

        <Tabs>
          <TabList>
            <Tab>{title}</Tab>
            <Tab>Yin</Tab>
            <Tab>Teresa</Tab>
            <Tab>Bryan</Tab>
          </TabList>

          <TabPanel>
            <ChoresTable assignee="All" chores={this.state.chores} />
          </TabPanel>
          <TabPanel>
            <ChoresTable
              assignee="Yin"
              chores={this.state.chores.filter((chore) => {
                return "Yin" === chore.assignee && chore;
              })}
            />
          </TabPanel>
          <TabPanel>
            <ChoresTable
              assignee="Teresa"
              chores={this.state.chores.filter((chore) => {
                return "Teresa" === chore.assignee && chore;
              })}
            />
          </TabPanel>
          <TabPanel>
            <ChoresTable
              assignee="Bryan"
              chores={this.state.chores.filter((chore) => {
                return "Bryan" === chore.assignee && chore;
              })}
            />
          </TabPanel>
        </Tabs>
        <div style={{ margin: "30px" }}>
          <ChoresUpdateButton></ChoresUpdateButton>
        </div>
      </Container>
    );
  }
}
