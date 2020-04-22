import React, { Component } from "react";
import "../App.css";
import APIRequest from "../Util/APIRequest";

import {
  TableContainer,
  TableHead,
  Table,
  TableRow,
  TableCell,
  TableBody,
} from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import moment from "moment";
import ChoresDialog from "./ChoresDialog";

export default class Chores extends Component {
  constructor(props) {
    super(props);
    // Temp values for testing without access to API
    this.state = {
      chores: [],
      members: {},
    };
    this.createChore = this.createChore.bind(this);
  }

  async getAllMembers() {
    const memberResult = await APIRequest.getFlatMembers();
    return memberResult.json();
  }

  async getAllChores() {
    const choreResult = await APIRequest.getChoresForFlat();
    return choreResult.json();
  }

  async deleteChore(id) {
    const deleteResult = await APIRequest.deleteChore(id);
    this.getAllChores().then((chores) => {
      const { members } = this.state;
      this.mapNamesToChores(chores, members);
    });
  }

  async markAsComplete(id) {
    const deleteResult = await APIRequest.markChoreComplete(id);
    this.getAllChores().then((chores) => {
      const { members } = this.state;
      this.mapNamesToChores(chores, members);
    });
  }

  componentDidMount() {
    this.getAllChores().then((chores) => {
      this.getAllMembers().then((members) => {
        this.mapNamesToChores(chores, members);
      });
    });
  }

  mapNamesToChores(chores, members) {
    chores.map((chore) => {
      let member = members.flatMembers.find(
        (member) => member.id === chore.assignee
      );
      if (member) {
        chore.name = member.firstName;
      }
      return chore;
    });
    this.setState({
      members: members,
      chores: chores,
    });
  }

  columns = [
    {
      id: 0,
      label: "",
    },
    {
      id: 1,
      label: "Monday",
    },
    {
      id: 2,
      label: "Tuesday",
    },
    {
      id: 3,
      label: "Wednesday",
    },
    {
      id: 4,
      label: "Thursday",
    },
    {
      id: 5,
      label: "Friday",
    },
    {
      id: 6,
      label: "Saturday",
    },
    {
      id: 7,
      label: "Sunday",
    },
  ];

  blankRow = [
    {
      id: 0,
    },
    {
      id: 1,
    },
    {
      id: 2,
    },
    {
      id: 3,
    },
    {
      id: 4,
    },
    {
      id: 5,
    },
    {
      id: 6,
    },
    {
      id: 7,
    },
  ];

  createChore(chore) {
    APIRequest.createChore(chore).then((result) => {
      this.getAllChores().then((chores) => {
        const { members } = this.state;
        this.mapNamesToChores(chores, members);
      });
    });
  }

  render() {
    const { chores, members } = this.state;
    return (
      <div>
        <p>New Chores</p>
        <TableContainer>
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                {this.columns.map((column) => {
                  return (
                    <TableCell
                      key={column.id}
                      style={({ textcolor: "white" }, { minWidth: 106 })}
                    >
                      {column.label}
                    </TableCell>
                  );
                })}
                <TableCell />
              </TableRow>
            </TableHead>
            <TableBody>
              {chores.map((chore) => {
                return (
                  <TableRow>
                    {this.blankRow.map((cell) => {
                      return (
                        <TableCell
                          style={
                            ({ color: "white" },
                            {
                              backgroundColor:
                                moment(chore.dueDate).format("dddd") ===
                                this.columns[cell.id].label
                                  ? chore.completed
                                    ? "green"
                                    : "red"
                                  : "white",
                            })
                          }
                          onClick={
                            moment(chore.dueDate).format("dddd") ===
                            this.columns[cell.id].label
                              ? () => this.markAsComplete(chore.id)
                              : null
                          }
                        >
                          {cell.id === 0 ? chore.title : ""}
                          {moment(chore.dueDate).format("dddd") ===
                          this.columns[cell.id].label
                            ? chore.name
                            : ""}
                        </TableCell>
                      );
                    })}
                    <TableCell
                      onClick={() => this.deleteChore(chore.id)}
                      style={{ backgroundColor: "white" }}
                    >
                      <DeleteIcon></DeleteIcon>
                    </TableCell>
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
        <ChoresDialog
          members={members}
          createChore={this.createChore}
        ></ChoresDialog>
      </div>
    );
  }
}
