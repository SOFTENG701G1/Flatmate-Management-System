import React, { Component } from "react";
import "../App.css";
import {
  TableContainer,
  TableHead,
  Table,
  TableRow,
  TableCell,
  TableBody,
} from "@material-ui/core";
import moment from "moment";
import ChoresDialog from "./ChoresDialog";

export default class Chores extends Component {
  constructor(props) {
    super(props);

    this.state = {
      chores: [
        {
          chore_id: 0,
          title: "Dishes",
          description: "do the dishes and put them away",
          assignee: 22,
          due_date: "2020-03-16T04:25:50.783Z",
          completed: 1,
          recurring: 0,
        },
        {
          chore_id: 1,
          title: "Washing",
          description: "do the washing and put them away",
          assignee: 23,
          due_date: "2020-03-18T03:25:50.783Z",
          completed: 0,
          recurring: 0,
        },
        {
          chore_id: 2,
          title: "Cleaning",
          description: "do the cleaning and put them away",
          assignee: 24,
          due_date: "2020-03-20T03:25:50.783Z",
          completed: 0,
          recurring: 0,
        },
      ],
    };
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

  render() {
    const { chores } = this.state;
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
                                moment(chore.due_date).format("dddd") ===
                                this.columns[cell.id].label
                                  ? chore.completed
                                    ? "green"
                                    : "red"
                                  : "white",
                            })
                          }
                        >
                          {cell.id === 0 ? chore.title : ""}
                          {moment(chore.due_date).format("dddd") ===
                          this.columns[cell.id].label
                            ? chore.assignee
                            : ""}
                        </TableCell>
                      );
                    })}
                  </TableRow>
                );
              })}
            </TableBody>
          </Table>
        </TableContainer>
        <ChoresDialog></ChoresDialog>
      </div>
    );
  }
}
