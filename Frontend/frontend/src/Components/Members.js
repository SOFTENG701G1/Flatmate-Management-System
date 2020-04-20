import React from "react";
import APIRequest from "../Util/APIRequest";
import "./Members.css";
import AddMember from "./AddMember";
import MemberTile from "./MemberTile";
import NewFlat from "./NewFlat";

export default class MembersPage extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      items: [],
      isLoaded: false,
    };
    this.getMember();
    this.setFlatState = this.setFlatState.bind(this);
    this.update = this.update.bind(this);
  }
  /* Function used to fetch JSON data from the API */
  async getMember() {
    const memberResult = await APIRequest.getFlatMembers();
    const json = await memberResult.json();
    this.setState({
      isLoaded: true,
      items: json,
    });
  }

  setFlatState(json) {
    this.setState({
      items: json,
    });
  }

  update() {
    window.location.reload();
  }

  render() {
    var { isLoaded, items } = this.state;

    if (!isLoaded) {
      return <div>Loading...</div>;
    } else {
      return (
        <div className="MembersPage">
          {/* If flatMembers array contains anything, the member is part of a flat, and so display member tiles. */}
          {items.flatMembers.length === 0 ? (
            <NewFlat setFlatState={this.setFlatState} />
          ) : (
            <div>
              <div className="section-header">Members page</div>
              <AddMember />
              <h4 className="currentMember">Current Members</h4>
              {this.state.items.flatMembers.map((flatMember, i) => {
                return (
                  <MemberTile
                    key={i}
                    flatMember={flatMember}
                    refresh={this.update.bind(this)}
                  />
                );
              })}
            </div>
          )}
        </div>
      );
    }
  }
}
