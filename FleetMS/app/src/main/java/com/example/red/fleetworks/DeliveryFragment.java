package com.example.red.fleetworks;


import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.auth0.android.jwt.JWT;
import com.example.red.fleetworks.Data.Delivery;
import com.example.red.fleetworks.Data.Pickups;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static android.content.Context.MODE_PRIVATE;


/**
 * A simple {@link Fragment} subclass.
 */
public class DeliveryFragment extends Fragment {

    ListView listView = null;
    View progressView = null;
    List<Delivery> Deliverylist = new ArrayList<Delivery>();
    public DeliveryFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view =  inflater.inflate(R.layout.fragment_pickups, container, false);
        final DeliveryAdapter DeliveryAdapter = new DeliveryAdapter(this.getActivity(), Deliverylist);
        listView = (ListView) view.findViewById(R.id.pickups_list_view);
        progressView =  view.findViewById(R.id.list_progress);
        RequestQueue queue = Volley.newRequestQueue(getActivity());
        showProgress(true);
        SharedPreferences prefs = getActivity().getSharedPreferences("LoginInfo", MODE_PRIVATE);
        String token = prefs.getString("token", null);
        JWT jwt = new JWT(token);
        final String userid = jwt.getClaim("unique_name").asString();

        String url ="http://fleetapi-dev.us-east-1.elasticbeanstalk.com/api/ShipmentOrders/";
        StringRequest stringRequest = new StringRequest(Request.Method.GET, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {

                        try {
                            JSONObject jsonObject = new JSONObject("{\"data\":"+response+"}");
                            JSONArray jsonArray = jsonObject.getJSONArray("data");
                            for (int i = 0; i < jsonArray.length(); i++) {
                                JSONObject jo = jsonArray.getJSONObject(i);
                                JSONObject address = jo.getJSONObject("shipment").getJSONObject("destination").getJSONObject("address");
                                String time = jo.getJSONObject("shipment").getString("originApptDatetime");
                                Deliverylist.add(new Delivery(jo.getInt("carrierId"), jo.getInt("driverId"), jo.getInt("id"), time, address.getString("streetname"), address.getString("city"), address.getString("province"), address.getString("postalcode")));
                                listView.setAdapter(DeliveryAdapter);
                                showProgress(false);
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }

                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Log.e("Fleet Error", error.getMessage());
                error.printStackTrace();
            }
        }){
            @Override
            public Map<String, String> getHeaders() throws AuthFailureError {
                Map<String, String> headers = new HashMap<>();
                SharedPreferences prefs = getActivity().getSharedPreferences("LoginInfo", MODE_PRIVATE);
                String token = prefs.getString("token", null);
                headers.put("Authorization", "Bearer " + token);
                headers.put("userid", userid);
                return headers;
            }
        };
        queue.add(stringRequest);
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Pickups pickup = (Pickups) DeliveryAdapter.getItem(position);
                Intent intent = new Intent(getActivity(), MainActivity.class);
                intent.putExtra("pickup", pickup);
                startActivity(intent);
            }
        });
        return view;
    }

    @TargetApi(Build.VERSION_CODES.HONEYCOMB_MR2)
    private void showProgress(final boolean show) {
        // On Honeycomb MR2 we have the ViewPropertyAnimator APIs, which allow
        // for very easy animations. If available, use these APIs to fade-in
        // the progress spinner.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2) {
            int shortAnimTime = getResources().getInteger(android.R.integer.config_shortAnimTime);

            listView.setVisibility(show ? View.GONE : View.VISIBLE);
            listView.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    listView.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });

            progressView.setVisibility(show ? View.VISIBLE : View.GONE);
            progressView.animate().setDuration(shortAnimTime).alpha(
                    show ? 1 : 0).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    progressView.setVisibility(show ? View.VISIBLE : View.GONE);
                }
            });
        } else {
            // The ViewPropertyAnimator APIs are not available, so simply show
            // and hide the relevant UI components.
            progressView.setVisibility(show ? View.VISIBLE : View.GONE);
            listView.setVisibility(show ? View.GONE : View.VISIBLE);
        }
    }

}
