using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Controllers;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using nkast.Aether.Physics2D.Dynamics.Joints;
using System;
using System.Collections.Generic;

namespace DualBlade._2D.Physics.Services;

public interface IPhysicsManager
{
    //
    // Summary:
    //     Set the user data. Use this to store your application specific data.
    //
    // Value:
    //     The user data.
    object Tag { get; set; }

    //
    // Summary:
    //     Fires whenever a body has been added
    BodyDelegate BodyAdded { get; set; }

    //
    // Summary:
    //     Fires whenever a body has been removed
    BodyDelegate BodyRemoved { get; set; }

    //
    // Summary:
    //     Fires whenever a fixture has been added
    FixtureDelegate FixtureAdded { get; set; }

    //
    // Summary:
    //     Fires whenever a fixture has been removed
    FixtureDelegate FixtureRemoved { get; set; }

    //
    // Summary:
    //     Fires whenever a joint has been added
    JointDelegate JointAdded { get; set; }

    //
    // Summary:
    //     Fires whenever a joint has been removed
    JointDelegate JointRemoved { get; set; }

    //
    // Summary:
    //     Fires every time a controller is added to the World.
    ControllerDelegate ControllerAdded { get; set; }

    //
    // Summary:
    //     Fires every time a controlelr is removed form the World.
    ControllerDelegate ControllerRemoved { get; set; }

    //
    // Summary:
    //     Get the contact manager for testing.
    //
    // Value:
    //     The contact manager.
    ContactManager ContactManager { get; }

    //
    // Summary:
    //     Get the world body list.
    //
    // Value:
    //     The head of the world body list.
    BodyCollection BodyList { get; }

    //
    // Summary:
    //     Get the world joint list.
    //
    // Value:
    //     The joint list.
    JointCollection JointList { get; }

    //
    // Summary:
    //     Get the number of broad-phase proxies.
    //
    // Value:
    //     The proxy count.
    int ProxyCount => ContactManager.BroadPhase.ProxyCount;

    //
    // Summary:
    //     Get the number of contacts (each may have 0 or more contact points).
    //
    // Value:
    //     The contact count.
    int ContactCount => ContactManager.ContactCount;

    //
    // Summary:
    //     Change the global gravity vector.
    //
    // Value:
    //     The gravity.
    Vector2 Gravity { get; set; }

    //
    // Summary:
    //     Is the world locked (in the middle of a time step).
    bool IsLocked { get; }

    //
    // Summary:
    //     Get the world contact list. ContactList is the head of a circular linked list.
    //     Use Contact.Next to get the next contact in the world list. A contact equal to
    //     ContactList indicates the end of the list.
    //
    // Value:
    //     The head of the world contact list.
    ContactListHead ContactList { get; }

    //
    // Summary:
    //     If false, the whole simulation stops. It still processes added and removed geometries.
    bool Enabled { get; set; }

    Island Island { get; }

    //
    // Summary:
    //     Add a rigid body. Warning: This method is locked during callbacks.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     Thrown when the world is Locked/Stepping.
    void Add(Body body);

    //
    // Summary:
    //     Destroy a rigid body. Warning: This automatically deletes all associated shapes
    //     and joints. Warning: This method is locked during callbacks.
    //
    // Parameters:
    //   body:
    //     The body.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     Thrown when the world is Locked/Stepping
    void Remove(Body body);

    //
    // Summary:
    //     Create a joint to constrain bodies together. This may cause the connected bodies
    //     to cease colliding. Warning: This method is locked during callbacks.
    //
    // Parameters:
    //   joint:
    //     The joint.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     Thrown when the world is Locked/Stepping.
    void Add(Joint joint);

    //
    // Summary:
    //     Destroy a joint. This may cause the connected bodies to begin colliding. Warning:
    //     This method is locked during callbacks.
    //
    // Parameters:
    //   joint:
    //     The joint.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     Thrown when the world is Locked/Stepping.
    void Remove(Joint joint);

    //
    // Summary:
    //     Add a rigid body.
    void AddAsync(Body body);

    //
    // Summary:
    //     Destroy a rigid body. Warning: This automatically deletes all associated shapes
    //     and joints.
    //
    // Parameters:
    //   body:
    //     The body.
    void RemoveAsync(Body body);

    //
    // Summary:
    //     Create a joint to constrain bodies together. This may cause the connected bodies
    //     to cease colliding.
    //
    // Parameters:
    //   joint:
    //     The joint.
    void AddAsync(Joint joint);

    //
    // Summary:
    //     Destroy a joint. This may cause the connected bodies to begin colliding.
    //
    // Parameters:
    //   joint:
    //     The joint.
    void RemoveAsync(Joint joint);

    //
    // Summary:
    //     All Async adds and removes are cached by the World during a World step. To process
    //     the changes before the world updates again, call this method.
    void ProcessChanges();

    //
    // Summary:
    //     Take a time step. This performs collision detection, integration, and consraint
    //     solution.
    //
    // Parameters:
    //   dt:
    //     The amount of time to simulate, this should not vary.
    void Step(TimeSpan dt);

    //
    // Summary:
    //     Take a time step. This performs collision detection, integration, and consraint
    //     solution.
    //
    // Parameters:
    //   dt:
    //     The amount of time to simulate, this should not vary.
    void Step(TimeSpan dt, ref SolverIterations iterations);

    //
    // Summary:
    //     Take a time step. This performs collision detection, integration, and consraint
    //     solution. Warning: This method is locked during callbacks.
    //
    // Parameters:
    //   dt:
    //     The amount of time to simulate in seconds, this should not vary.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     Thrown when the world is Locked/Stepping.
    public void Step(float dt);

    //
    // Summary:
    //     Take a time step. This performs collision detection, integration, and consraint
    //     solution. Warning: This method is locked during callbacks.
    //
    // Parameters:
    //   dt:
    //     The amount of time to simulate in seconds, this should not vary.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     Thrown when the world is Locked/Stepping.
    void Step(float dt, ref SolverIterations iterations);

    //
    // Summary:
    //     Call this after you are done with time steps to clear the forces. You normally
    //     call this after each call to Step, unless you are performing sub-steps. By default,
    //     forces will be automatically cleared, so you don't need to call this function.
    void ClearForces();

    //
    // Summary:
    //     Query the world for all fixtures that potentially overlap the provided AABB.
    //     Inside the callback: Return true: Continues the query Return false: Terminate
    //     the query
    //
    // Parameters:
    //   callback:
    //     A user implemented callback class.
    //
    //   aabb:
    //     The aabb query box.
    void QueryAABB(QueryReportFixtureDelegate callback, AABB aabb);

    // Summary:
    //     Query the world for all fixtures that potentially overlap the provided AABB.
    //     Inside the callback: Return true: Continues the query Return false: Terminate
    //     the query
    //
    // Parameters:
    //   callback:
    //     A user implemented callback class.
    //
    //   aabb:
    //     The aabb query box.
    public void QueryAABB(QueryReportFixtureDelegate callback, ref AABB aabb);

    // Summary:
    //     Ray-cast the world for all fixtures in the path of the ray. Your callback controls
    //     whether you get the closest point, any point, or n-points. The ray-cast ignores
    //     shapes that contain the starting point. Inside the callback: return -1: ignore
    //     this fixture and continue return 0: terminate the ray cast return fraction: clip
    //     the ray to this point return 1: don't clip the ray and continue
    //
    // Parameters:
    //   callback:
    //     A user implemented callback class.
    //
    //   point1:
    //     The ray starting point.
    //
    //   point2:
    //     The ray ending point.
    void RayCast(RayCastReportFixtureDelegate callback, Vector2 point1, Vector2 point2);

    // Summary:
    //     Warning: This method is locked during callbacks.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     Thrown when the world is Locked/Stepping.
    void Add(Controller controller);

    Fixture TestPoint(Vector2 point);

    void ShiftOrigin(Vector2 newOrigin);

    //
    // Summary:
    //     Warning: This method is locked during callbacks.
    //
    // Exceptions:
    //   T:System.InvalidOperationException:
    //     Thrown when the world is Locked/Stepping.
    void Remove(Controller controller);

    Body CreateBody(Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);
    void Clear();
    Body CreateEdge(Vector2 start, Vector2 end);
    Body CreateChainShape(Vertices vertices, Vector2 position = default);
    Body CreateLoopShape(Vertices vertices, Vector2 position = default);
    Body CreateRectangle(float width, float height, float density, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);
    Body CreateCircle(float radius, float density, Vector2 position = default, BodyType bodyType = BodyType.Static);
    Body CreateEllipse(float xRadius, float yRadius, int edges, float density, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);
    Body CreatePolygon(Vertices vertices, float density, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);

    Body CreateCompoundPolygon(List<Vertices> list, float density, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);

    Body CreateGear(float radius, int numberOfTeeth, float tipPercentage, float toothHeight, float density, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);

    Body CreateCapsule(float height, float topRadius, int topEdges, float bottomRadius, int bottomEdges, float density, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);

    Body CreateCapsule(float height, float endRadius, float density, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);

    Body CreateRoundedRectangle(float width, float height, float xRadius, float yRadius, int segments, float density, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);

    Body CreateLineArc(float radians, int sides, float radius, bool closed = false, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);

    Body CreateSolidArc(float density, float radians, int sides, float radius, Vector2 position = default, float rotation = 0f, BodyType bodyType = BodyType.Static);

    Path CreateChain(Vector2 start, Vector2 end, float linkWidth, float linkHeight, int numberOfLinks, float linkDensity, bool attachRopeJoint);
}