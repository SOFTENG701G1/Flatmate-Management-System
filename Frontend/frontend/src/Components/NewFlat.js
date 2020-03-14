import React from 'react';
import { withRouter } from 'react-router-dom'
import { Route } from 'react-router-dom'

export default class NewFlat extends React.Component{

    render(){

        const Button = withRouter(({ history }) => (
            <button
              type='button'
              onClick={() => { history.push('/members') }}
            >
              Create Flat
            </button>
          ))

        return (
            
            <div>
                <h2 class="title">You are not currently part of a flat</h2>
                <p class="subheading">Press the "Create Flat" Button to create a new flat</p>
                <Button />
            </div>
        );
    }

}






