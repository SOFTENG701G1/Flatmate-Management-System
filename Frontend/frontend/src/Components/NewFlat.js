import React from 'react';
import './NewFlat.css';
import User from '../Util/User'
import APIRequest from '../Util/APIRequest';
import { Redirect } from 'react-router-dom';
import { useHistory } from "react-router-dom";

export default class NewFlat extends React.Component {

  render() {
    if (User.getFlatState().flatMembers.length > 0) {
      return <Redirect to="/app/members" />
    } else {
      return (

        <div>
          <h2 class="title">You are not currently part of a flat</h2>
          <p class="subheading">Press the "Create Flat" Button to create a new flat</p>
          <HomeButton />
        </div>
      );
    }
  }

}


function HomeButton() {
  const history = useHistory();

  async function handleClick() {
    var resp = await APIRequest.createNewFlat();    
    if(resp.status == 201){
      var flat = await resp.json();
      User.setFlatState(flat);
    } 
    history.push("/app/members") 
  }

  return (
    <button type="button" onClick={handleClick}>
      Create Flat
    </button>
  );
}






