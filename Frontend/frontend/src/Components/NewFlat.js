import React from 'react';
import './NewFlat.css';
import APIRequest from '../Util/APIRequest';
import { useHistory } from "react-router-dom";
import MembersPage from './AddMember';

export default class NewFlat extends React.Component {
  constructor (props) {
    super(props);
    
    this.handleClick = this.handleClick.bind(this);
  }

  async handleClick(){
    var resp = await APIRequest.createNewFlat();
    if (resp.status == 201) {
      var flat = await resp.json();
      this.props.setFlatState(flat);
    }
  }

  render() {
    return (

      <div>
        <h2 class="title">You are not currently part of a flat</h2>
        <p class="subheading">Press the "Create Flat" Button to create a new flat</p>
        <button className="HomeButton" onClick={this.handleClick}>Create Flat</button>
      </div>
    );
  }
}







