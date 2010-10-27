function CPlane()
{
}

CPlane.prototype =
{
	n: null,
    d: null,

    setPointNormal: function( point, normal )
    {
		this.setNormalDistance( normal, -normal.dot( point ) );
    },

    setNormalDistance: function( normal, d )
    {
        this.n = normal;
        this.d = d;
    },

	distance: function( point )
    {
        return this.n.elements[0] * point.elements[0] + this.n.elements[1] * point.elements[1] + this.n.elements[2] * point.elements[2] + this.d;
    },

    normalize: function()
    {
        var mag = Math.sqrt( this.n.elements[0] * this.n.elements[0] + this.n.elements[1] * this.n.elements[1] + this.n.elements[2] * this.n.elements[2] );
        return CPlane.create( this.normal / mag, this.d / mag );
    }
};

CPlane.create = function( normal, d )
{
	var p = new CPlane();
	p.setNormalDistance( normal, d );
	return p;
};

CPlane.createAnchor = function( point, normal )
{
	var p = new CPlane();
	p.setPointNormal( point, normal );
	return p;
};